using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class SubClassesRepository: TravelRepository<SubClass>, ISubClassesRepository
    {
        public SubClassesRepository(TravelContext travelContext) : base(travelContext)
        {
            
        }
    }
}