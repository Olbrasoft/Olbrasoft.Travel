using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class LocalizedPointsOfInterestRepository : TravelRepository<LocalizedPointOfInterest>, ILocalizedPointsOfInterestRepository
    {
        public LocalizedPointsOfInterestRepository(TravelContext travelContext) : base(travelContext)
        {
        }
    }
}