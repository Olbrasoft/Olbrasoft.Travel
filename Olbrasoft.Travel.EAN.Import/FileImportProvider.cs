using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Olbrasoft.Travel.EAN.Import
{
    class FileImportProvider : IImportProvider
    {
        public string GetFirstLine(string path)
        {
            return File.ReadLines(path).First();
        }

        public int GetCountLines(string path)
        {
            return File.ReadLines(path).Count();
        }

        public IEnumerable<string> GetBatchLines(string path, int batch, int batchSize)
        {
            return File.ReadLines(path).Skip((batch - 1) * batchSize + 1).Take(batchSize);
        }
    }
}