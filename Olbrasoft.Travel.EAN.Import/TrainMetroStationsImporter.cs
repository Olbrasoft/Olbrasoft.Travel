using System.Collections.Generic;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class TrainMetroStationsImporter : Importer<TrainMetroStationCoordinates>
    {
        public TrainMetroStationsImporter(IProvider provider, IParserFactory parserFactory, IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger) 
            : base(provider, parserFactory, factoryOfRepositories, sharedProperties, logger)
        {
        }

        public override void Import(string path)
        {
            LoadData(path);

            var eanIdsToIds = ImportRegions(EanEntities, FactoryOfRepositories.Regions(), CreatorId);

            ImportLocalizedRegions(EanEntities, FactoryOfRepositories.Localized<LocalizedRegion>(), eanIdsToIds,
                DefaultLanguageId, CreatorId);
            
            ImportRegionsToTypes(EanEntities, FactoryOfRepositories.RegionsToTypes(), eanIdsToIds,
                FactoryOfRepositories.BaseNames<TypeOfRegion>().GetId("Train Station"),
                FactoryOfRepositories.BaseNames<SubClass>().GetId("train"), CreatorId);
        }



        private void ImportRegionsToTypes(
            IEnumerable<TrainMetroStationCoordinates> trainsMetroStationCoordinateses,
            IRegionsToTypesRepository repository,
            IReadOnlyDictionary<long, int> eanIdsToIds, 
            int typeOfRegionTrainStationId, 
            int subClassTrainId,
            int creatorId
            )
        {
            LogBuild<RegionToType>();

            var regionsToTypes = BuildRegionsToTypes(trainsMetroStationCoordinateses, eanIdsToIds,
                typeOfRegionTrainStationId, subClassTrainId, creatorId);

            var count = regionsToTypes.Length;

            LogBuilded(count);

            if (count <= 0) return;

            LogSave<RegionToType>();
            repository.BulkSave(regionsToTypes);
            LogSaved<RegionToType>();
        }


        private void ImportLocalizedRegions(
            IEnumerable<TrainMetroStationCoordinates> trainsMetroStationCoordinateses,
            ILocalizedRepository<LocalizedRegion> repository,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            int languageId,
            int creatorId

            )
        {
            LogBuild<LocalizedRegion>();
            var localizedRegions = BuildLocalizedRegions(trainsMetroStationCoordinateses, eanIdsToIds, languageId, creatorId);
            var count = localizedRegions.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<LocalizedRegion>();
            repository.BulkSave(localizedRegions, count);
            LogSaved<LocalizedRegion>();
        }


        private IReadOnlyDictionary<long, int> ImportRegions(
            IEnumerable<TrainMetroStationCoordinates> trainsMetroStationCoordinateses,
            IRegionsRepository repository,
            int creatorId
            )
        {
            LogBuild<Region>();
            var regions = BuildRegions(trainsMetroStationCoordinateses, creatorId);
            var count = regions.Length;
            LogBuilded(count);

            if (count <= 0) return repository.EanIdsToIds;

            LogSave<Region>();
            repository.BulkSave(regions, r => r.Coordinates);
            LogSaved<Region>();

            return repository.EanIdsToIds;

        }
    }




}