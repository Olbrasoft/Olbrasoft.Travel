using System.Collections.Generic;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class PointsOfInterestCoordinatesBatchImporter : BatchImporter<PointOfInterestCoordinates>
    {
        public PointsOfInterestCoordinatesBatchImporter(ImportOption option) : base(option)
        {
        }

        public override void ImportBatch(PointOfInterestCoordinates[] pointsOfInterestCoordinates)
        {
            var regionsRepository = FactoryOfRepositories.Regions();
            var typeOfRegionPointOfInterestId = FactoryOfRepositories.BaseNames<TypeOfRegion>().GetId("Point of Interest");

            LogBuild<Region>();
            var regions = BuildRegions(pointsOfInterestCoordinates, CreatorId);
            var count = regions.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<Region>();
            regionsRepository.BulkSave(regions, r => r.Coordinates);
            LogSaved<Region>();

            var eanIdsToIds = regionsRepository.EanIdsToIds;

            LogBuild<RegionToType>();
            var regionsToTypes = BuildRegionsToTypes(pointsOfInterestCoordinates, eanIdsToIds,
                FactoryOfRepositories.BaseNames<SubClass>().NamesToIds, typeOfRegionPointOfInterestId, CreatorId);
            count = regionsToTypes.Length;
            LogBuilded(count);

            if (count > 0)
            {
                LogSave<RegionToType>();
                FactoryOfRepositories.RegionsToTypes().BulkSave(regionsToTypes);
                LogSaved<RegionToType>();
            }

            LogBuild<LocalizedRegion>();
            var localizedRegions = BuildLocalizedRegions(pointsOfInterestCoordinates, eanIdsToIds,
                DefaultLanguageId, CreatorId);
            count = localizedRegions.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<LocalizedRegion>();
            FactoryOfRepositories.Localized<LocalizedRegion>().BulkSave(localizedRegions);
            LogSaved<LocalizedRegion>();
        }


        private static RegionToType[] BuildRegionsToTypes(IEnumerable<PointOfInterestCoordinates> pointsOfInterestCoordinates,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            IReadOnlyDictionary<string, int> subClasses,
            int typeOfRegionId,
            int creatorId
            )
        {
            var regionsToTypes= new Queue<RegionToType>();

            foreach (var eanPoint in pointsOfInterestCoordinates)
            {
                if (!eanIdsToIds.TryGetValue(eanPoint.RegionID, out var id)) continue;

                var regionToType = new RegionToType
                {
                    Id=id,
                    ToId = typeOfRegionId,
                    CreatorId = creatorId
                };

                var subClassName = GetSubClassName(eanPoint.SubClassification);
                
                if (!string.IsNullOrEmpty(subClassName) && subClasses.TryGetValue(subClassName , out var subClassId))
                {
                    regionToType.SubClassId = subClassId;
                }

                regionsToTypes.Enqueue(regionToType);
            }
            return regionsToTypes.ToArray();
        }

        
        private static LocalizedRegion[] BuildLocalizedRegions(IEnumerable<PointOfInterestCoordinates> pointsOfInterestCoordinates,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            int languageId,
            int creatorId
            )
        {
            var localizedRegions = new Queue<LocalizedRegion>();

            foreach (var eanPoint in pointsOfInterestCoordinates)
            {
                if (!eanIdsToIds.TryGetValue(eanPoint.RegionID, out var id)) continue;

                var localizedRegion = new LocalizedRegion
                {
                    Id = id,
                    LanguageId = languageId,
                    Name = eanPoint.RegionName,
                    LongName = eanPoint.RegionNameLong,
                    CreatorId = creatorId
                };

                localizedRegions.Enqueue(localizedRegion);
            }

            return localizedRegions.ToArray();
        }


       
    }
}