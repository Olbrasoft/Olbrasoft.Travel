using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class LanguagesRepository : BaseRepository<Language>, ILanguagesRepository {
        public LanguagesRepository(TravelContext context) : base(context)
        {
        }

        public override void ClearCache()
        {
           
        }
    }
}