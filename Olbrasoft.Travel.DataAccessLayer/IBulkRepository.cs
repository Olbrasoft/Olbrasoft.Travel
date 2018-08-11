using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Olbrasoft.Travel.DataAccessLayer
{
    public interface IBulkRepository<T> where T : class
    {
        void BulkSave(IEnumerable<T> entities, params Expression<Func<T, object>>[] ignorePropertiesWhenUpdating);

        void BulkSave(IEnumerable<T> entities, int batchSize, params Expression<Func<T, object>>[] ignorePropertiesWhenUpdating);
    }

}