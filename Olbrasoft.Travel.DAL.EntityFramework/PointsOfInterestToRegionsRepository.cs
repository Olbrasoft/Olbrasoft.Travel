using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class PointsOfInterestToRegionsRepository : TravelRepository<PointOfInterestToRegion>, IPointsOfInterestToRegionsRepository {
        public PointsOfInterestToRegionsRepository(TravelContext travelContext) : base(travelContext)
        {

        }
    }
}