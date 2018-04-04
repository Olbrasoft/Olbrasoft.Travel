using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Olbrasoft.Travel.EAN.Import
{
    internal abstract class BatchImporter<T>:BaseImporter<T> where T : class, new()
    {
        protected int BatchSize;
        protected int BatchCount;

        protected BatchImporter(ImportOption option):base(option)
        {
            BatchSize = Option.ImportBatchSize;
        }

        //public static IEnumerable<List<T>> SplitToEanumerableOfList<T>(IEnumerable<T> locations, int nSize = 90000)
        //{
        //    var result = locations.ToList();
        //    for (var i = 0; i < result.Count; i += nSize)
        //    {
        //        yield return result.GetRange(i, Math.Min(nSize, result.Count - i));
        //    }
        //}
        
        public override void Import(string path)
        {
            var countLines = Provider.GetCountLines(path);
            BatchCount = GetBatchCount(countLines, BatchSize);

            WriteLog($"Start Import {typeof(T)} from:{path} import batch size is:{BatchSize} count lines is:{countLines} batch count is:{BatchCount}.");

            var parser = CreateParser(path);
            
            for (var batch = 1; batch <= BatchCount; batch++)
            {
                var lines = Provider.GetBatchLines(path, batch, BatchSize).ToArray();
                var entities = parser.Parse(lines);
                ImportBatch(entities.ToArray());
            }
        }

        
        public abstract void ImportBatch(T[] eanEntities );
       
        public static int GetBatchCount(int countLines, int batchSize = 90000, bool skipFirstLine = true)
        {
            if (skipFirstLine) countLines = countLines - 1;
            return (int)Math.Ceiling(countLines / (double)batchSize);
        }

    }



}
