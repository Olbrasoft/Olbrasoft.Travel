using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class LanguagesRepository : TravelRepository<Language>, ILanguagesRepository {
        public LanguagesRepository(TravelContext travelContext) : base(travelContext)
        {
        }
    }
}