using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class TypesOfRegionsRepository : TravelRepository<TypeOfRegion>, ITypesOfRegionsRepository {
        public TypesOfRegionsRepository(TravelContext travelContext) : base(travelContext)
        {
        }
    }
}