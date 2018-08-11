using Olbrasoft.Travel.Data.Entity;
using Olbrasoft.Travel.DAL.EntityFramework;

namespace Olbrasoft.Travel.DataAccessLayer.EntityFramework
{
    public class RegionsToRegionsRepository : ManyToManyRepository<RegionToRegion>, IRegionsToRegionsRepository
    {
        public RegionsToRegionsRepository(TravelContext context) : base(context)
        {

        }
    }
}