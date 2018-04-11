using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IMappedEntitiesRepository<T> : IBulkRepository<T>, IBaseRepository<T> where T : class, IHaveEanId<int>
    {
        HashSet<int>EanIds { get; }
        IReadOnlyDictionary<int, int> EanIdsToIds { get; }

        void BulkSave(IEnumerable<T> entities, int batchSize,  params Expression<Func<T, object>>[] ignorePropertiesWhenUpdating);
    }
}