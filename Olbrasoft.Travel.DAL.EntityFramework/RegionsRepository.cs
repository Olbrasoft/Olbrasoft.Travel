using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class RegionsRepository : BaseRegionsRepository<Region>, IRegionsRepository
    {
        public RegionsRepository(TravelContext context) : base(context)
        {
        }
    }
}