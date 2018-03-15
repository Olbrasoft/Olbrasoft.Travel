using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class LocalizedRepository<T> : BaseRepository<T, int, int>, ILocalizedRepository<T> where T : Localized
    {
        public LocalizedRepository(DbContext context) : base(context)
        {
        }

        public override void ClearCache()
        {
        }

        public override void BulkSave(IEnumerable<T> entities)
        {
            var entitiesArray = entities as T[] ?? entities.ToArray();
            foreach (var languageId in entitiesArray.GroupBy(entity => entity.LanguageId).Select(grp => grp.First()).Select(p=>p.LanguageId))
            {
                if (!Exists(languageId))
                {
                    BulkInsert(entitiesArray);
                }
                else
                {
                    var storedLocalizedIds =
                        new HashSet<int>(FindIds(languageId));

                    var forInsert = new List<T>();
                    var forUpdate = new List<T>();

                    foreach (var entity in entitiesArray)
                    {
                        if (!storedLocalizedIds.Contains(entity.Id))
                        {
                            forInsert.Add(entity);
                        }
                        else
                        {
                            forUpdate.Add(entity);
                        }
                    }

                    if (forInsert.Count > 0)
                    {
                       BulkInsert(forInsert);
                    }

                    if (forUpdate.Count <= 0) return;
                    BulkUpdate(forUpdate);
                }
            }
        }

        protected void BulkUpdate(IEnumerable<T> entities)
        {
           Context.BulkUpdate(entities,OnSaved);
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