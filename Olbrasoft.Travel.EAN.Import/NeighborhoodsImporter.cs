using Olbrasoft.Travel.BLL;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{
    class NeighborhoodsImporter : CitiesNeighborhoodsImporter<Neighborhood>
    {
        public NeighborhoodsImporter(ParentRegionImportOption option) : base(option)
        {
        }

        protected override void SetTypeOfRegionIdAndSubClassId(IRegionsFacade regionsFacade, ISubClassesFacade subClassesFacade)
        {
            WriteLog("It will be imported Neighborhoods.");

            if (RegionsFacade.TypesOfRegionsAsReverseDictionary().TryGetValue("Neighborhood", out var typeOfRegionCityId))
            {
                TypeOfRegionId = typeOfRegionCityId;
            }

            if (SubClassesFacade.SubClassesAsReverseDictionary().TryGetValue("neighbor", out var subClassCityId))
            {
                SubClassId = subClassCityId;
            }
        }
    }
}