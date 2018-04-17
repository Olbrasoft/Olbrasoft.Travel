using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class NeighborhoodsImporter : RegionsTypesOfCitiesAndNeighborhoodsImporter
    {
        public NeighborhoodsImporter(IProvider provider,  IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger)
            : base(provider, factoryOfRepositories, sharedProperties, logger)
        {
            TypeOfRegionId = FactoryOfRepositories.BaseNames<TypeOfRegion>().GetId("Neighborhood");
            SubClassId = FactoryOfRepositories.BaseNames<SubClass>().GetId("neighbor");
        }

    }
}