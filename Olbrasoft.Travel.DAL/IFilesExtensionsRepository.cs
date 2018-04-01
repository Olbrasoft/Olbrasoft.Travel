using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IFilesExtensionsRepository : IBaseRepository<FileExtension>
    {
        IReadOnlyDictionary<string, int> ExtensionsToIds { get; }
        void Save(IEnumerable<FileExtension> filesExtensions);
    }
}
