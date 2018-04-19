using System.Collections.Generic;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class RegionsCenterImporter : Importer<RegionCenter>
    {
        public RegionsCenterImporter(IProvider provider, IParserFactory parserFactory, IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger) 
            : base(provider, parserFactory, factoryOfRepositories, sharedProperties, logger)
        {
        }

        public override void Import(string path)
        {
            LoadData(path);

            var eanIdsToIds = ImportRegions(EanEntities, FactoryOfRepositories.Regions(), CreatorId);
            
            ImportLocalizedRegions(EanEntities, FactoryOfRepositories.Localized<LocalizedRegion>(), eanIdsToIds,
                DefaultLanguageId, CreatorId);
            
            EanEntities = null;
        }
        
        private IReadOnlyDictionary<long, int> ImportRegions(
            IEnumerable<RegionCenter> regionsCenter,
            IRegionsRepository repository,
            int creatorId
        )
        {
            LogBuild<Region>();
            var regions = BuildRegions(regionsCenter, creatorId);
            var count = regions.Length;
            LogBuilded(count);

            if (count <= 0) return repository.EanIdsToIds;

            LogSave<Region>();
            repository.BulkSave(regions, r => r.Coordinates);
            LogSaved<Region>();

            return repository.EanIdsToIds;
        }
        
        private static Region[] BuildRegions(IEnumerable<RegionCenter> eanEntities,
            int creatorId
        )
        {
            var regions = new Queue<Region>();
            foreach (var eanEntity in eanEntities)
            {
                var region = new Region
                {
                    CenterCoordinates = CreatePoint(eanEntity.CenterLatitude, eanEntity.CenterLongitude),
                    EanId = eanEntity.RegionID,
                    CreatorId = creatorId
                };

                regions.Enqueue(region);
            }
            return regions.ToArray();
        }
    }
}