using System.Collections.Generic;
using Olbrasoft.Travel.Data.Entity;

namespace Olbrasoft.Travel.DataAccessLayer
{
    public interface IFilesExtensionsRepository : IBaseRepository<FileExtension>
    {
        HashSet<string> Extensions { get; }
        IReadOnlyDictionary<string, int> ExtensionsToIds { get; }
        void Save(IEnumerable<FileExtension> filesExtensions);
    }
}
