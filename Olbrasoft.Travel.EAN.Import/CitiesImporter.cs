using Olbrasoft.Travel.BLL;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{
    class CitiesImporter : CitiesNeighborhoodsImporter<City> {
        public CitiesImporter(ImportOption option) : base(option)
        {
        }

        protected override void SetTypeOfRegionIdAndSubClassId(IRegionsFacade regionsFacade, ISubClassesFacade subClassesFacade)
        {
            WriteLog("It will be imported Cities.");

            if (RegionsFacade.TypesOfRegionsAsReverseDictionary().TryGetValue("City", out var typeOfRegionCityId))
            {
                TypeOfRegionId = typeOfRegionCityId;
            }

            if (SubClassesFacade.SubClassesAsReverseDictionary().TryGetValue("city", out var subClassCityId))
            {
                SubClassId = subClassCityId;
            }
        }
    }



}
