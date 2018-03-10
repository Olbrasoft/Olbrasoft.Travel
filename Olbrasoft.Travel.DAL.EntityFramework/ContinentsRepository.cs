using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class ContinentsRepository:BaseRegionsRepository<Continent>,IContinentsRepository
    {
        public ContinentsRepository(TravelContext context) : base(context)
        {
            

        }
    }
}