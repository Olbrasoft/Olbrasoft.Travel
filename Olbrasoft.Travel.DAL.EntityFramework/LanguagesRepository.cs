using System.Data.Entity;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class LanguagesRepository : BaseRepository<Language>, ILanguagesRepository {

        public LanguagesRepository(DbContext context) : base(context)
        {
        }
        
        
    }
}