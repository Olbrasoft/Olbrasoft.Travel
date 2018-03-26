using System.Collections.Generic;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class RegionCenterBatchImporter : BatchImporter<RegionCenter>
    {
        public RegionCenterBatchImporter(ImportOption option) : base(option)
        {
        }

        public override void ImportBatch(RegionCenter[] eanEntities)
        {
            var regionsRepository = FactoryOfRepositories.Regions();
            
            LogBuild<Region>();
            var regions = BuildRegions(eanEntities,CreatorId);
            var count = regions.Length;
            LogBuilded(count);

            if (count > 0)
            {
                LogSave<Region>();
                regionsRepository.BulkSave(regions, r => r.Coordinates);
                LogSaved<Region>();
            }

            var eanIdsToIds = regionsRepository.EanIdsToIds;

            LogBuild<LocalizedRegion>();
            var localizedRegions = BuildLocalizedRegions(eanEntities, eanIdsToIds, DefaultLanguageId, CreatorId);
            count = localizedRegions.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<LocalizedRegion>();
            FactoryOfRepositories.Localized<LocalizedRegion>().BulkSave(localizedRegions);
            LogSaved<LocalizedRegion>();

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