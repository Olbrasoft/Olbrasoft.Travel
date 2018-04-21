using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class FileImportProvider : IProvider
    {
        public event EventHandler<string[]> SplittingLine;

        protected virtual void OnSplittingLine(string[] e)
        {
            SplittingLine?.Invoke(this, e);
        }

        public void ReadToEnd(string path)
        {
            using (var reader = new StreamReader(path))
            {
                //skip first line
                 reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    OnSplittingLine(reader.ReadLine()?.Split('|'));
                }
            }
        }

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

        public IEnumerable<string> GetAllLines(string path)
        {
            return File.ReadAllLines(path);
        }
    }
}