using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class TypesOfRegionsRepository : BaseNamesRepository<TypeOfRegion>, ITypesOfRegionsRepository
    {
        protected new TravelContext Context;

        public TypesOfRegionsRepository(TravelContext context) : base(context)
        {
            Context = context;
        }
    }
}