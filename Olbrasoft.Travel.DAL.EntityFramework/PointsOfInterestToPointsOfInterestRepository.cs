using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class PointsOfInterestToPointsOfInterestRepository :ManyToManyRepository<PointOfInterestToPointOfInterest>, IPointsOfInterestToPointsOfInterestRepository
    {
        public PointsOfInterestToPointsOfInterestRepository(TravelContext context) : base(context)
        {
        }
    }
}