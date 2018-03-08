using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class RegionsRepository : KeyIdRepository<Region>, IRegionsRepository
    {
        public RegionsRepository(TravelContext travelContext) : base(travelContext)
        {
        }
    }
}