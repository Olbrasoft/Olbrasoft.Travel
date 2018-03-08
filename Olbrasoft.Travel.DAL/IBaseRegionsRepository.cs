using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IBaseRegionsRepository<T> : IKeyIdRepository<T> where T: BaseRegion
    {
        IReadOnlyDictionary<long, int> EanRegionIdsToIds { get; }
    }
}