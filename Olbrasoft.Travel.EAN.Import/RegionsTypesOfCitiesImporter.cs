using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class RegionsTypesOfCitiesImporter : RegionsTypesOfCitiesAndNeighborhoodsImporter
    {
        public RegionsTypesOfCitiesImporter(IProvider provider, IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger) 
            : base(provider, factoryOfRepositories, sharedProperties, logger)
        {
            TypeOfRegionId = FactoryOfRepositories.BaseNames<TypeOfRegion>().GetId("City");
            SubClassId = FactoryOfRepositories.BaseNames<SubClass>().GetId("city");
        }
    }
}