using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Olbrasoft.EntityFramework.Bulk;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class TravelRepository<T> : SharpRepository.EfRepository.EfRepository<T>, ITravelRepository<T> where T : class
    {
        public event EventHandler<EventArgs> OnSaved;

        protected new readonly TravelContext Context;
        
        
        public TravelRepository(TravelContext travelContext) : base(travelContext)
        {
           Context = travelContext;
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
            OnSaved?.Invoke(this, new EventArgs());
        }

        public new void Add(IEnumerable<T> entities)
        {
            base.Add(entities);
            OnSaved?.Invoke(this, new EventArgs());
        }

        public virtual void BulkInsert(IEnumerable<T> entities)
        {
            BulkInsert(entities.ToArray());
        }

        public virtual void BulkInsert(T[] entities)
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

                OnSaved?.Invoke(this, new EventArgs());
            }
        }

        public void BulkUpdate(IEnumerable<T> entities)
        {
            BulkUpdate(entities.ToArray());

        }



        public virtual void BulkUpdate(T[] entities)
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
                OnSaved?.Invoke(this, new EventArgs());
            }
        }

        public virtual void BulkInsertOrUpdate(T[] entities)
        {
            var batchesToInsertOrUpdate = SplitList(entities, 90000);

            foreach (var batch in batchesToInsertOrUpdate)
            {
                Context.BulkInsertOrUpdate(batch, new BulkConfig()
                {
                    BatchSize = 45000,
                    BulkCopyTimeout = 480,
                    IgnoreColumns = new HashSet<string>(new[] { "DateAndTimeOfCreation" })
                });
                OnSaved?.Invoke(this, new EventArgs());
            }
        }


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
