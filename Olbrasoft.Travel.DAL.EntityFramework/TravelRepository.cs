using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Olbrasoft.EntityFramework.Bulk;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class TravelRepository<T> :SharpRepository.EfRepository.EfRepository<T>, ITravelRepository<T> where T : class
    {
        protected new readonly TravelContext Context;

        public TravelRepository(TravelContext travelContext) : base(travelContext)
        {
            Context = travelContext;
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

        public void BulkInsert(IEnumerable<T> entities)
        {
            var batchesToInsert = SplitList(entities, 90000);

            foreach (var batch in batchesToInsert)
            {
                Context.BulkInsert(batch, new BulkConfig() { BatchSize = 45000, BulkCopyTimeout = 480 });
            }
        }

        public void BulkUpdate(IEnumerable<T> entities)
        {
            var batchesToUpdate = SplitList(entities, 90000);

            foreach (var batch in batchesToUpdate)
            {
                Context.BulkUpdate(batch, new BulkConfig() { BatchSize = 45000, BulkCopyTimeout = 480 });
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
