using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class TypesOfRegionsRepository : BaseNamesRepository<TypeOfRegion>, ITypesOfRegionsRepository
    {
        public TypesOfRegionsRepository(TravelContext context) : base(context)
        {
        }
    }
}