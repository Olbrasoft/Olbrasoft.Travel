using System.Collections.Generic;
using Olbrasoft.Travel.DTO;
using SharpRepository.Repository;

namespace Olbrasoft.Travel.DAL
{
    public interface IBaseRegionsRepository<T> : IBaseRepository<T> where T : BaseRegion
    {
        IEnumerable<long> EanRegionIds { get; }
        IReadOnlyDictionary<long, int> EanRegionIdsToIds { get; }
    }
}