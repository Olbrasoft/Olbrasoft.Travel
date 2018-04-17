using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Globalization;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class PointsOfInterestImporter : Importer
    {
        private IReadOnlyDictionary<string, int> _subClassesNamesToIds;

        protected IReadOnlyDictionary<string, int> SubClassesNamesToIds
        {
            get => _subClassesNamesToIds ??
                   (_subClassesNamesToIds = FactoryOfRepositories.BaseNames<SubClass>().NamesToIds);

            set => _subClassesNamesToIds = value;
        }

        protected Queue<Region> Regions = new Queue<Region>();
        protected IDictionary<long, Tuple<string, string>> AdeptsToLocalizedRegions = new Dictionary<long, Tuple<string, string>>();
        protected IDictionary<long, int> RegionsEanIdsToSubClassIds = new Dictionary<long, int>();

        public PointsOfInterestImporter(IProvider provider, IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger)
            : base(provider, factoryOfRepositories, sharedProperties, logger)
        {
        }


        protected override void RowLoaded(string[] items)
        {
            if (!long.TryParse(items[0], out var eanRegionId) ||
               !double.TryParse(items[3], NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out var latitude) ||
                !double.TryParse(items[4], NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out var longitude)) return;

            AdeptsToLocalizedRegions.Add(eanRegionId, new Tuple<string, string>(items[1], items[2]));

            Regions.Enqueue(new Region
            {
                EanId = eanRegionId,
                CenterCoordinates = CreatePoint(latitude, longitude),
                CreatorId = CreatorId
            });

            var subClassName = GetSubClassName(items[5]);


            if (string.IsNullOrEmpty(subClassName) || !SubClassesNamesToIds.TryGetValue(subClassName, out var subClassId)) return;

            RegionsEanIdsToSubClassIds.Add(eanRegionId, subClassId);

        }

        public override void Import(string path)
        {
            LogBuild<Region>();

            LoadData(path);
            
            SubClassesNamesToIds = null;

            var eanIdsToIds = ImportRegions(Regions.ToArray(), FactoryOfRepositories.Regions(), 100000, p => p.Coordinates);

            Regions = null;

            ImportLocalizedRegions(AdeptsToLocalizedRegions, FactoryOfRepositories.Localized<LocalizedRegion>(), eanIdsToIds, DefaultLanguageId, CreatorId);

            var regionsEanIds = AdeptsToLocalizedRegions.Keys;

            AdeptsToLocalizedRegions = null;

            ImportRegionsToTypes(regionsEanIds, FactoryOfRepositories.RegionsToTypes(), RegionsEanIdsToSubClassIds,
                eanIdsToIds, FactoryOfRepositories.BaseNames<TypeOfRegion>().GetId("Point of Interest"), CreatorId);

        }

        private void ImportRegionsToTypes(
            IEnumerable<long> regionsEanIds,
            IRegionsToTypesRepository repository,
            IDictionary<long, int> regionsEanIdsToSubClassIds,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            int typeOfRegionId,
            int creatorId
            )
        {

            LogBuild<RegionToType>();
            var regionsToTypes = BuildRegionsToTypes(regionsEanIds, regionsEanIdsToSubClassIds, eanIdsToIds, typeOfRegionId, creatorId);
            var count = regionsToTypes.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<RegionToType>();
            repository.BulkSave(regionsToTypes,count);
            LogSaved<RegionToType>();
        }

        private static RegionToType[] BuildRegionsToTypes(
            IEnumerable<long> regionsEanIds,
            IDictionary<long, int> regionsEanIdsToSubClassIds,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            int typeOfRegionId,
            int creatorId
            )
        {

            var regionsToTypes = new Queue<RegionToType>();

            foreach (var regionEanId in regionsEanIds)
            {
                if (!eanIdsToIds.TryGetValue(regionEanId, out var id)) continue;

                var regionToType = new RegionToType
                {
                    Id = id,
                    ToId = typeOfRegionId,
                    CreatorId = creatorId
                };

                if (regionsEanIdsToSubClassIds.TryGetValue(regionEanId, out var subClassId))
                {
                    regionToType.SubClassId = subClassId;
                }

                regionsToTypes.Enqueue(regionToType);
            }

            return regionsToTypes.ToArray();
        }

        public static DbGeography CreatePoint(double latitude, double longitude)
        {
            var point = string.Format(CultureInfo.InvariantCulture.NumberFormat,
                "POINT({0} {1})", longitude, latitude);
            // 4326 is most common coordinate system used by GPS/Maps
            return DbGeography.PointFromText(point, 4326);
        }
    }
}