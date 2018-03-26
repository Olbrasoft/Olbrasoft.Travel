using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class LanguagesRepository : BaseRepository<Language>, ILanguagesRepository
    {
        private IReadOnlyDictionary<string, int> _eanLanguageCodesToIds;

        public IReadOnlyDictionary<string, int> EanLanguageCodesToIds
        {
            get
            {
                return _eanLanguageCodesToIds ?? (_eanLanguageCodesToIds = AsQueryable()
                           .Select(l => new {l.EanLanguageCode, l.Id}).ToDictionary(k => k.EanLanguageCode, v => v.Id));
            }
            
            private set => _eanLanguageCodesToIds = value;
        }

        public LanguagesRepository(DbContext context) : base(context)
        {

        }
        
        public override void ClearCache()
        {
            EanLanguageCodesToIds = null;
            base.ClearCache();
        }

    }
}