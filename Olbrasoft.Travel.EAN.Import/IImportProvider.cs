using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Olbrasoft.Travel.EAN.Import
{
    interface IImportProvider
    {
        string GetFirstLine(string path);

        int GetCountLines(string path);

        IEnumerable<string> GetBatchLines(string path, int batch, int batchSize);
    }
}
