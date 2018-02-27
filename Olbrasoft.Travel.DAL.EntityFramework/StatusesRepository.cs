using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class StatusesRepository: TravelRepository<Status>,IStatusesRepository
    {
        public StatusesRepository(TravelContext travelContext) : base(travelContext)
        {
        }
    }
}