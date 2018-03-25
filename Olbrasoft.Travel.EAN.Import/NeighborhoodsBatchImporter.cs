using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class NeighborhoodsBatchImporter : BaseCitiesAndNeighborhoodImporter<DTO.Geography.NeighborhoodCoordinates>
    {
        public NeighborhoodsBatchImporter(ImportOption option) : base(option)
        {
            TypeOfRegionId = FactoryOfRepositories.BaseNames<TypeOfRegion>().GetId("Neighborhood");
            SubClassId = FactoryOfRepositories.BaseNames<SubClass>().GetId("neighbor");
        }
    }
}