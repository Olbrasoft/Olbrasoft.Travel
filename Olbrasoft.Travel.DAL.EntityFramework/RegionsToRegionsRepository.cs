using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class RegionsToRegionsRepository : ManyToManyRepository<RegionToRegion>, IRegionsToRegionsRepository
    {
        public RegionsToRegionsRepository(TravelContext context) : base(context)
        {

        }
    }
}