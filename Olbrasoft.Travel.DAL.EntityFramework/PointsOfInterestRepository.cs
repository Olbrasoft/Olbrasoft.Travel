using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class PointsOfInterestRepository : GeoRepository<PointOfInterest>, IPointsOfInterestRepository
    {
        protected new TravelContext Context;
        
        public PointsOfInterestRepository(TravelContext context) : base(context)
        {
            Context = context;
        }
        
    }
}
