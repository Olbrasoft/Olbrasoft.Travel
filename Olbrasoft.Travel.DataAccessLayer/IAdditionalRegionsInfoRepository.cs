using System.Collections.Generic;
using Olbrasoft.Travel.Data.Entity;

namespace Olbrasoft.Travel.DataAccessLayer
{
    public interface IAdditionalRegionsInfoRepository<T> : IBaseRepository<T>, IBulkRepository<T>
        where T : CreatorInfo, IAdditionalRegionInfo
    {
        IReadOnlyDictionary<string, int> CodesToIds { get; }

    }


}