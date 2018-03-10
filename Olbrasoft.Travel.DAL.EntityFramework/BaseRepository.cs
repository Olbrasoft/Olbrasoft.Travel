using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Olbrasoft.EntityFramework.Bulk;

namespace Olbrasoft.Travel.DAL.EntityFramework
{

    #region BaseRepository<T, TKey, TKey2>
    public abstract class BaseRepository<T, TKey, TKey2> : SharpRepository.EfRepository.EfRepository<T, TKey, TKey2>, IBaseRepository<T, TKey, TKey2> where T : class
    {
        public event EventHandler Saved;
      
        protected BaseRepository(DbContext context) : base(context)
        {
        }


        public new void Add(IEnumerable<T> entities)
        {
            base.Add(entities);
            OnSaved(EventArgs.Empty);
        }

        public new void Add(T entity)
        {
            base.Add(entity);
            OnSaved(EventArgs.Empty);
        }
        
        public void BulkInsert(IEnumerable<T> entities)
        {
            var batchesToInsert = BaseRepository<T>.SplitList(entities, 90000);

            foreach (var batch in batchesToInsert)
            {
                Context.BulkInsert(batch,
                    new BulkConfig
                    {
                        BatchSize = 45000,
                        BulkCopyTimeout = 480,
                        IgnoreColumns = new HashSet<string>(new[] { "DateAndTimeOfCreation" })

                    });

                OnSaved(EventArgs.Empty);
            }

        }

        protected void OnSaved(EventArgs eventArgs)
        {
            var handler = Saved;
            handler?.Invoke(this, eventArgs);
            ClearCache();
        }

        public abstract void ClearCache();

    }

    #endregion


    public abstract class BaseRepository<T> : SharpRepository.EfRepository.EfRepository<T>, IBaseRepository<T> where T : class
    {
        public event EventHandler Saved;
        
        protected BaseRepository(DbContext context) : base(context)
        {
           
        }

        public new TResult Min<TResult>(Expression<Func<T, TResult>> selector)
        {
            return Context.Set<T>().Min(selector);
        }

        public T Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includePaths)
        {
            var query = Context.Set<T>().Where(predicate);

            if (includePaths != null)
            {
                query = includePaths.Aggregate(query,
                    (current, include) => current.Include(include));
            }

            return query.AsNoTracking().FirstOrDefault();

        }

        public new void Add(T entity)
        {
            base.Add(entity);
            OnSaved(EventArgs.Empty);
        }

        public new void Add(IEnumerable<T> entities)
        {
            base.Add(entities);
            OnSaved(EventArgs.Empty);
        }

        protected virtual void BulkInsert(IEnumerable<T> entities)
        {
            var batchesToInsert = SplitList(entities, 90000);

            foreach (var batch in batchesToInsert)
            {
                Context.BulkInsert(batch,
                    new BulkConfig
                    {
                        BatchSize = 45000,
                        BulkCopyTimeout = 480,
                        IgnoreColumns = new HashSet<string>(new[] { "DateAndTimeOfCreation" })

                    });

                OnSaved(EventArgs.Empty);
            }
        }

        protected void BulkUpdate(IEnumerable<T> entities)
        {
            var batchesToUpdate = SplitList(entities, 90000);

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

        public virtual void BulkSave(IEnumerable<T> entities)
        {
            var entitiesArray = entities as T[] ?? entities.ToArray();
            BulkInsert(entitiesArray.Where(p => GetPrimaryKey(p) == 0));
            BulkUpdate(entitiesArray.Where(p => GetPrimaryKey(p) != 0));
        }

        protected void OnSaved(EventArgs eventArgs)
        {
            var handler = Saved;
            handler?.Invoke(this, eventArgs);
            ClearCache();
            base.ClearCache();
        }
        
        public new abstract void ClearCache();
       
        public static IEnumerable<List<T>> SplitList(IEnumerable<T> locations, int nSize = 30)
        {
            var result = locations.ToList();
            for (var i = 0; i < result.Count; i += nSize)
            {
                yield return result.GetRange(i, Math.Min(nSize, result.Count - i));
            }
        }
    }

}
