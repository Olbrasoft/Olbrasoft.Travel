using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IBaseNamesRepository<T> : IKeyIdRepository<T> where T : BaseName
    {
        IReadOnlyDictionary<string,int> NamesToIds { get; }
    }
}