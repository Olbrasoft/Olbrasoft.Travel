using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Olbrasoft.Travel.DAL
{
    public interface ITravelRepository<T> : SharpRepository.Repository.IRepository<T> where T : class
    {
        T Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includePaths);
        void BulkInsert(IEnumerable<T> entities);
        void BulkUpdate(IEnumerable<T> entities);
    }
}
