using System.Collections.Generic;
using Olbrasoft.Travel.Data.Entity;

namespace Olbrasoft.Travel.DataAccessLayer
{
    public interface IPathsToPhotosRepository : IBulkRepository<PathToPhoto>, IBaseRepository<PathToPhoto>
    {
        IReadOnlyDictionary<string, int> PathsToIds { get; }
        HashSet<string> Paths { get; }
    }
}