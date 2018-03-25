using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface ITypesRepository<T> : IBulkRepository<T> where T : BaseName
    {
        int GetId(string name);
        IEnumerable<string> Names { get; }
        IReadOnlyDictionary<string, int> NamesToIds { get; }
    }
}