using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class TrainMetroStationsBatchImporter:BatchImporter<TrainMetroStationCoordinates>
    {
        public TrainMetroStationsBatchImporter(ImportOption option) : base(option)
        {
        }

        public override void ImportBatch(TrainMetroStationCoordinates[] eanEntities)
        {
            var regionsRepository = FactoryOfRepositories.Regions();

            LogBuild<Region>();
            var regions = BuildRegions(eanEntities, CreatorId);
            var count = regions.Length;
            LogBuilded(count);

            if (count > 0)
            {
                LogSave<Region>();
                regionsRepository.BulkSave(regions, r => r.Coordinates);
                LogSaved<Region>();
            }
            
            var eanIdsToIds = regionsRepository.EanIdsToIds;
            
            var typeOfRegionTrainStationId = FactoryOfRepositories.BaseNames<TypeOfRegion>().GetId("Train Station");
            var subClassTrainId = FactoryOfRepositories.BaseNames<SubClass>().GetId("train");

            LogBuild<RegionToType>();
            var regionsToTypes = BuildRegionsToTypes(eanEntities, eanIdsToIds, typeOfRegionTrainStationId,
                subClassTrainId, CreatorId);
            count = regionsToTypes.Length;
            LogBuilded(count);

            if (count > 0)
            {
                LogSave<RegionToType>();
                FactoryOfRepositories.RegionsToTypes().BulkSave(regionsToTypes);
                LogSaved<RegionToType>();
            }

            LogBuild<LocalizedRegion>();
            var localizedRegions = BuildLocalizedRegions(eanEntities, eanIdsToIds, DefaultLanguageId, CreatorId);
            count = localizedRegions.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<LocalizedRegion>();
            FactoryOfRepositories.Localized<LocalizedRegion>().BulkSave(localizedRegions);
            LogSaved<LocalizedRegion>();

        }
        


        //private static  Region[]  BuildRegions(IEnumerable<TrainMetroStationCoordinates> eanTrainStations, 
        //    int creatorId
        //)
        //{
        //    var regions = new Queue<Region>();

        //    foreach (var eanTrainStation in eanTrainStations)
        //    {
        //        var region = new Region
        //        {
        //            CenterCoordinates = CreatePoint(eanTrainStation.Latitude,eanTrainStation.Longitude),
        //            EanId = eanTrainStation.RegionID,
        //            CreatorId = creatorId
        //        };

        //        regions.Enqueue(region);
        //    }

        //    return regions.ToArray();
        //}
    }
}