using System.Collections.Generic;

namespace Olbrasoft.Travel.EAN.Import
{
    public interface IImportProvider
    {
        string GetFirstLine(string path);

        int GetCountLines(string path);

        IEnumerable<string> GetBatchLines(string path, int batch, int batchSize);

        IEnumerable<string> GetAllLines(string path);
    }
}
