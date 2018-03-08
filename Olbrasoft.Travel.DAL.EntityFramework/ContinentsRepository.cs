using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class ContinentsRepository:TravelRepository<Continent>,IContinentsRepository
    {
        public ContinentsRepository(TravelContext travelContext) : base(travelContext)
        {
            

        }
    }
}