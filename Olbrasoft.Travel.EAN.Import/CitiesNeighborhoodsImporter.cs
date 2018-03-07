using System;
using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.BLL;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{
    abstract class CitiesNeighborhoodsImporter<T> : Importer<T> where T : CityNeighborhood, new()
    {

        protected new readonly ParentRegionImportOption Option;
        protected IRegionsFacade RegionsFacade;
        protected ISubClassesFacade SubClassesFacade;
        protected int TypeOfRegionId;
        protected int SubClassId;

        
        protected CitiesNeighborhoodsImporter(ParentRegionImportOption option) : base(option)
        {
            Option = option;
            RegionsFacade = Option.RegionsFacade;
            SubClassesFacade = Option.SubClassesFacade;
            
        }

        protected abstract void SetTypeOfRegionIdAndSubClassId(IRegionsFacade regionsFacade,
            ISubClassesFacade subClassesFacade);


        protected override void ImportBatch(IEnumerable<T> eanPointsOfInterest)
        {

            SetTypeOfRegionIdAndSubClassId(RegionsFacade, SubClassesFacade);

            if (TypeOfRegionId == 0)
            {
                WriteLog($"{nameof(TypeOfRegionId)} is 0 import will be terminated.");
                return;
            }

            if (SubClassId == 0)
            {
                WriteLog($"{nameof(SubClassId)} is 0 import will be terminated.");
                return;
            }

            var storedRegions = RegionsFacade.GetMappingEanRegionIdsToRegions(true);

            var defaltBaseRegion = new BaseRegion { TypeOfRegionId = TypeOfRegionId, SubClassId = SubClassId, CreatorId = CreatorId };

            WriteLog("Regions Build.");
            var regions = BuildRegions(eanPointsOfInterest, storedRegions, defaltBaseRegion, out var adeptsToLocalizedRegions);
            var count = regions.Length;
            WriteLog($"Regions Builded:{count}.");

            if (count > 0)
            {
                WriteLog("Regions Save.");
                RegionsFacade.BulkSave(regions);
                WriteLog("Regions Saved.");
            }

            WriteLog("LocalizedRegions Build.");
            var localizedRegions = BuildLocalizedRegions(adeptsToLocalizedRegions,
                RegionsFacade.GetMappingEanRegionIdsToIds(true), CreatorId, DefaultLanguageId);

            count = localizedRegions.Length;
            WriteLog($"LocalizedRegions Builded:{count}.");

            if (count <= 0) return;
            WriteLog("LocalizedRegions Save.");
            Option.LocalizedFacade.BulkSave(localizedRegions);
            WriteLog("LocalizedRegions Saved.");
        }

        private static LocalizedRegion[] BuildLocalizedRegions(IDictionary<long, string> adeptsToLocalizedRegions,
            IDictionary<long, int> mappingRegionIdsToIds, int creatorId, int defaultLanguageId)
        {
            var localizedRegions = new Dictionary<int, LocalizedRegion>();
            foreach (var parentRegion in adeptsToLocalizedRegions)
            {
                if (!mappingRegionIdsToIds.TryGetValue(parentRegion.Key, out var regionId)) continue;
                if (localizedRegions.ContainsKey(regionId)) continue;
                var localizedRegion = new LocalizedRegion()
                {
                    RegionId = regionId,
                    Name = parentRegion.Value,
                    CreatorId = creatorId,
                    LanguageId = defaultLanguageId,
                    DateAndTimeOfCreation = DateTime.Now
                };

                localizedRegions.Add(regionId, localizedRegion);

            }

            return localizedRegions.Values.ToArray();
        }


        private static Region[] BuildRegions(
            IEnumerable<CityNeighborhood> eanEntities,
            IDictionary<long, BaseRegion> storedRegions,
            BaseRegion defaultBaseRegion,
            out IDictionary<long, string> adeptsToLocalizedRegions
        )
        {
            var regions = new HashSet<Region>();
            adeptsToLocalizedRegions = new Dictionary<long, string>();
            foreach (var city in eanEntities)
            {
                var region = new Region { EanRegionId = city.RegionID };
                if (storedRegions.TryGetValue(city.RegionID, out var baseRegion))
                {
                    region.Id = baseRegion.Id;
                    region.SubClassId = baseRegion.SubClassId;
                    region.TypeOfRegionId = baseRegion.TypeOfRegionId;
                    region.CreatorId = baseRegion.CreatorId;
                    region.DateAndTimeOfCreation = baseRegion.DateAndTimeOfCreation;
                }
                else
                {
                    region.SubClassId = defaultBaseRegion.SubClassId;
                    region.TypeOfRegionId = defaultBaseRegion.TypeOfRegionId;
                    region.CreatorId = defaultBaseRegion.CreatorId;
                    region.DateAndTimeOfCreation = DateTime.Now;

                    if (!adeptsToLocalizedRegions.ContainsKey(city.RegionID))
                    {
                        adeptsToLocalizedRegions.Add(city.RegionID, city.RegionName);
                    }
                }

                region.Coordinates = CreatePoligon(city.Coordinates);
                regions.Add(region);
            }

            return regions.ToArray();
        }
    }
}