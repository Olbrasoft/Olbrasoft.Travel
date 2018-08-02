using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IAdditionalRegionsInfoRepository<T> : IBaseRepository<T>, IBulkRepository<T>
        where T : CreatorInfo, IAdditionalRegionInfo
    {
        IReadOnlyDictionary<string, int> CodesToIds { get; }

    }


}