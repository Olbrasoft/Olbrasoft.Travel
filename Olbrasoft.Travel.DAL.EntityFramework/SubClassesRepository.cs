using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class SubClassesRepository: BaseNamesRepository<SubClass>, ISubClassesRepository
    {
        public SubClassesRepository(TravelContext context) : base(context)
        {
            
        }
    }
}