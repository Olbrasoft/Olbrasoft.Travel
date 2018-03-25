using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IRegionsRepository : IBulkRepository<Region>, IBaseRepository<Region>
    {
        IReadOnlyDictionary<long, int> EanIdsToIds { get; }
       
    }
}
