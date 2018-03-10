using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class PointsOfInterestToRegionsRepository : ManyToManyRepository<PointOfInterestToRegion>, IPointsOfInterestToRegionsRepository {
        public PointsOfInterestToRegionsRepository(TravelContext context) : base(context)
        {

        }
    }
}