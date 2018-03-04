using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class PointsOfInterestToPointsOfInterestRepository : TravelRepository<PointOfInterestToPointOfInterest>, IPointsOfInterestToPointsOfInterestRepository
    {
        public PointsOfInterestToPointsOfInterestRepository(TravelContext travelContext) : base(travelContext)
        {
        }
    }
}