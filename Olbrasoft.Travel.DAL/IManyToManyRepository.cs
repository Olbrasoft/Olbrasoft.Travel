using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IManyToManyRepository<T> : IBulkRepository<T>, IBaseRepository<T, int, int> where T : ManyToMany
    {
        void BulkSave(IEnumerable<T> manyToManyEntities,int batchSize,
            params Expression<Func<T, object>>[] ignorePropertiesWhenUpdating);
    }
    
}