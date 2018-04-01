﻿using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IMappedEntitiesRepository<T> : IBulkRepository<T>, IBaseRepository<T> where T : class, IHaveEanId<int>
    {
        HashSet<int>EanIds { get; }
        IReadOnlyDictionary<int, int> EanIdsToIds { get; }
    }
}