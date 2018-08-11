using Olbrasoft.Travel.Data.Entity;
using Olbrasoft.Travel.DataAccessLayer;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.Import
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