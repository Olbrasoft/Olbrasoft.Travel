using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class CitiesBatchImporter : BaseCitiesAndNeighborhoodImporter<CityCoordinates>
    {
        public CitiesBatchImporter(ImportOption option) : base(option)
        {
            TypeOfRegionId = FactoryOfRepositories.BaseNames<TypeOfRegion>().GetId("City");
            SubClassId = FactoryOfRepositories.BaseNames<SubClass>().GetId("city");
        }

      
    }

}
