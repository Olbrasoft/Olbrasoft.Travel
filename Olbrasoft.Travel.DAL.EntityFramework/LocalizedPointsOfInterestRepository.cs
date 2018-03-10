using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class LocalizedPointsOfInterestRepository : LocalizedRepository<LocalizedPointOfInterest>, ILocalizedPointsOfInterestRepository
    {
        public LocalizedPointsOfInterestRepository(TravelContext context) : base(context)
        {
        }
    }
}