﻿using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IBaseNamesRepository<T> : IBaseRepository<T> where T : BaseName
    {
        IEnumerable<string> Names { get; }
        IReadOnlyDictionary<string, int> NamesToIds { get; }
    }
}