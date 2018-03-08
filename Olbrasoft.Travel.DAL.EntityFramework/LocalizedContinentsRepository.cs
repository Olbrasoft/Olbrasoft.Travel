using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class LocalizedContinentsRepository : TravelRepository<LocalizedContinent>, ILocalizedContinentsRepository {
        public LocalizedContinentsRepository(TravelContext travelContext) : base(travelContext)
        {
        }
    }
}