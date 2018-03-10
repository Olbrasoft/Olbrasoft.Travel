using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Olbrasoft.Travel.DAL
{
    public interface IBaseRepository<T> : SharpRepository.Repository.IRepository<T> where T : class
    {
        T Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includePaths);

        void BulkSave(IEnumerable<T> entities);

    }

    public interface IBaseRepository<T, TKey, TKey2> : SharpRepository.Repository.ICompoundKeyRepository<T, TKey, TKey2>,IBulkInsert<T>,ICanClearCache
        where T : class
    {
        
    }

}
