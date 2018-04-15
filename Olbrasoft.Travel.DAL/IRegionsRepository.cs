using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IRegionsRepository : IBulkRepository<Region>, IBaseRepository<Region>
    {
        IReadOnlyDictionary<long, int> EanIdsToIds { get; }

        void BulkSave(IEnumerable<Region> regions, int batchSize,
            params Expression<Func<Region, object>>[] ignorePropertiesWhenUpdating);
        
    }
}
