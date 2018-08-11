using Olbrasoft.Travel.Data.Entity;

namespace Olbrasoft.Travel.DataAccessLayer.EntityFramework
{
    public class RegionsToRegionsRepository : ManyToManyRepository<RegionToRegion>, IRegionsToRegionsRepository
    {
        public RegionsToRegionsRepository(TravelContext context) : base(context)
        {

        }
    }
}