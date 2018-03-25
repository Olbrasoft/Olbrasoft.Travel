using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Olbrasoft.Travel.DAL
{
    public interface IBaseRepository<T> : SharpRepository.Repository.IRepository<T> where T : class
    {
        T Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includePaths);

       // void BulkSave(IEnumerable<T> entities, params Expression<Func<T,object>>[] ignorePropertiesWhenUpdating);
        
    }

    public interface IBaseRepository<T, TKey, TKey2> : SharpRepository.Repository.ICompoundKeyRepository<T, TKey, TKey2>,ICanClearCache
        where T : class
    {
       // void BulkSave(IEnumerable<T> entities, params Expression<Func<T, object>>[] ignorePropertiesWhenUpdating);
       
    }

}
