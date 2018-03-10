using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class LocalizedContinentsRepository : LocalizedRepository<LocalizedContinent>, ILocalizedContinentsRepository {
        public LocalizedContinentsRepository(TravelContext context) : base(context)
        {
        }
    }
}