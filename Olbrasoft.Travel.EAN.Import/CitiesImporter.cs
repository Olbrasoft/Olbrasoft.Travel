using Olbrasoft.Travel.BLL;
using Olbrasoft.Travel.EAN.DTO.Geography;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.EAN.Import
{
    class CitiesImporter : Importer<City>
    {
        protected new readonly ParentRegionImportOption Option;
        protected IRegionsFacade RegionsFacade;
        protected ISubClassesFacade SubClassesFacade;
        protected readonly int TypeOfRegionCityId;
        protected int SubClassCityId;

        public CitiesImporter(ParentRegionImportOption option) : base(option)
        {
            Option = option;
            RegionsFacade = Option.RegionsFacade;
            SubClassesFacade = Option.SubClassesFacade;

            if (RegionsFacade.TypesOfRegionsAsReverseDictionary().TryGetValue("City", out var typeOfRegionCityId))
            {
                TypeOfRegionCityId = typeOfRegionCityId;
            }

            if (SubClassesFacade.SubClassesAsReverseDictionary().TryGetValue("city", out var subClassCityId))
            {
                SubClassCityId = subClassCityId;
            }
        }
        
        protected override void ImportBatch(IEnumerable<City> cities)
        {
            if (TypeOfRegionCityId == 0)
            {
                WriteLog($"{nameof(TypeOfRegionCityId)} is 0 import will be terminated.");
                return;
            }

            if (SubClassCityId == 0)
            {
                WriteLog($"{nameof(SubClassCityId)} is 0 import will be terminated.");
                return;
            } 
            
            var storedRegions = RegionsFacade.GetMappingEanRegionIdsToRegions(true);
            
            var defaltBaseRegion = new BaseRegion { TypeOfRegionId = TypeOfRegionCityId, SubClassId = SubClassCityId, CreatorId = CreatorId };

            WriteLog("Cities Build.");
            var regions = BuildRegions(cities, storedRegions, defaltBaseRegion, out var adeptsToLocalizedRegions);
            var count = regions.Length;
            WriteLog($"Cities Builded:{count}.");



        }
        

        public Region[] BuildRegions(
            IEnumerable<City> cities,
            IDictionary<long, BaseRegion> storedRegions,
            BaseRegion defaultBaseRegion,
            out IDictionary<long, string> adeptsToLocalizedRegions
            )
        {
            var regions = new HashSet<Region>();
            adeptsToLocalizedRegions = new Dictionary<long, string>();
            foreach (var city in cities)
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

                region.Coordinates = ParsePolygon(city.Coordinates);
                regions.Add(region);
            }

            return regions.ToArray();
        }


       
    }



}
