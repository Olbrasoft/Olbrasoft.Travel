using System;
using System.Collections.Generic;
using Olbrasoft.EntityFramework.Bulk;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class LocalizedRepository<T> : BaseRepository<T, int, int>, ILocalizedRepository<T> where T : BaseLocalized
    {
        public LocalizedRepository(TravelContext context) : base(context)
        {
        }

        public override void ClearCache()
        {
        }

        public void BulkUpdate(IEnumerable<T> entities)
        {
            var batchesToUpdate = BaseRepository<T>.SplitList(entities, 90000);

            foreach (var batch in batchesToUpdate)
            {
                Context.BulkUpdate(batch, new BulkConfig()
                {
                    BatchSize = 45000,
                    BulkCopyTimeout = 480,
                    IgnoreColumns = new HashSet<string>(new[] { "DateAndTimeOfCreation" }),
                    IgnoreColumnsUpdate = new HashSet<string>(new[] { "CreatorId" })
                });

                OnSaved(EventArgs.Empty);
            }
        }

        public bool Exists(int languageId)
        {
            return Exists(l => l.LanguageId == languageId);
        }

        public IEnumerable<int> FindIds(int languageId)
        {
            return FindAll(l => l.LanguageId == languageId, l => l.Id);
        }
    }
}