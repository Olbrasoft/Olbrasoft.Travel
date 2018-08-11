using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using Olbrasoft.Travel.Data.Entity;
using Olbrasoft.Travel.DataAccessLayer;
using Olbrasoft.Travel.EAN.Import;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.Import
{
    internal class RegionsTypesOfCitiesAndNeighborhoodsImporter : Importer
    {

        protected int TypeOfRegionId;
        protected int SubClassId;

        protected Queue<Region> Regions = new Queue<Region>();
 
        protected IDictionary<long, Tuple<string, string>> AdeptsToLocalizedRegions = new Dictionary<long, Tuple<string, string>>();

        public RegionsTypesOfCitiesAndNeighborhoodsImporter(IProvider provider, IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger)
            : base(provider, factoryOfRepositories, sharedProperties, logger)
        {
        }

        protected override void RowLoaded(string[] items)
        {
            if (!long.TryParse(items[0], out var eanRegionId)) return;

            AdeptsToLocalizedRegions.Add(eanRegionId, new Tuple<string, string>(items[1], null));

            var region = new Region
            {
                EanId = eanRegionId,
                Coordinates = CreatePoligon(items[2]),
                CreatorId = CreatorId
            };

            Regions.Enqueue(region);
        }

        public override void Import(string path)
        {
            LogBuild<Region>();

            LoadData(path);

            var eanIdsToIds = ImportRegions(Regions.ToArray(), FactoryOfRepositories.Regions(), 90000, p => p.CenterCoordinates);

            Regions = null;

            ImportLocalizedRegions(AdeptsToLocalizedRegions, FactoryOfRepositories.Localized<LocalizedRegion>(), eanIdsToIds, DefaultLanguageId, CreatorId);

            var regionsEanIds = AdeptsToLocalizedRegions.Keys;

            AdeptsToLocalizedRegions = null;

            ImportRegionsToTypes(regionsEanIds, FactoryOfRepositories.RegionsToTypes(), eanIdsToIds, TypeOfRegionId,
                SubClassId, CreatorId);

        }
        
        private void ImportRegionsToTypes(
            ICollection<long> regionsEanIds,
            IRegionsToTypesRepository repository,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            int typeOfRegionId,
            int subClassId,
            int creatorId
            )
        {
            LogBuild<RegionToType>();
            var regionsToTypes = BuildRegionsToTypes(regionsEanIds, eanIdsToIds, typeOfRegionId, subClassId, creatorId);
            var count = regionsToTypes.Length;
            LogBuilded(count);

            if (count <= 0) return;

            LogSave<RegionToType>();
            repository.BulkSave(regionsToTypes, count, rtt => rtt.SubClassId);
            LogSaved<RegionToType>();
        }
        


        protected RegionToType[] BuildRegionsToTypes(
            ICollection<long> regionsEanIds,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            int typeOfRegionId,
            int subClassId,
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
                    SubClassId = subClassId,
                    CreatorId = creatorId
                };

                regionsToTypes.Enqueue(regionToType);
            }

            return regionsToTypes.ToArray();
        }

        public DbGeography CreatePoligon(string s)
        {
            var spl = s.Split(':');
            var pointsString = new StringBuilder();

            foreach (var s1 in spl)
            {
                var latLon = s1.Split(';');
                // var lotLanString = $"{latLon[1]} {latLon[0]},";
                pointsString.Append($"{latLon[1]} {latLon[0]},");
            }

            pointsString.Append(pointsString.ToString().Split(',').First());

            return DbGeography.PolygonFromText($"POLYGON(({pointsString}))", 4326);
        }
    }
}