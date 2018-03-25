using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class SubClassesRepository: TypesRepository<SubClass>, ISubClassesRepository
    {
        public SubClassesRepository(TravelContext context) : base(context)
        {
            
        }
    }
}