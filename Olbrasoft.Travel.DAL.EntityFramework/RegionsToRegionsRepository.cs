using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class RegionsToRegionsRepository : TravelRepository<RegionToRegion>, IRegionsToRegionsRepository
    {
        public RegionsToRegionsRepository(TravelContext travelContext) : base(travelContext)
        {

        }
    }
}