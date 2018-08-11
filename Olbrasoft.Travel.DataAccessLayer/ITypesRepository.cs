using System.Collections.Generic;
using Olbrasoft.Travel.Data.Entity;

namespace Olbrasoft.Travel.DataAccessLayer
{
    public interface ITypesRepository<T> : SharpRepository.Repository.IRepository<T>, IBulkRepository<T> where T : class, IHaveName
    {
        int GetId(string name);
        IEnumerable<string> Names { get; }
        IReadOnlyDictionary<string, int> NamesToIds { get; }
    }
}