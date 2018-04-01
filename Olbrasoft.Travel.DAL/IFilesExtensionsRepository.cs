using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IFilesExtensionsRepository : IBaseRepository<FileExtension>
    {
        HashSet<string> Extensions { get; }
        IReadOnlyDictionary<string, int> ExtensionsToIds { get; }
        void Save(IEnumerable<FileExtension> filesExtensions);
    }
}
