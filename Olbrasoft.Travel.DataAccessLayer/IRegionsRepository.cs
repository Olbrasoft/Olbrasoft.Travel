using System.Collections.Generic;
using Olbrasoft.Travel.Data.Entity;

namespace Olbrasoft.Travel.DataAccessLayer
{
    public interface IRegionsRepository : IBulkRepository<Region>, IBaseRepository<Region>
    {
        IReadOnlyDictionary<long, int> EanIdsToIds { get; }

    
        
    }
}
