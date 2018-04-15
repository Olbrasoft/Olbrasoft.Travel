using System;
using System.IO;
using Olbrasoft.Travel.DAL;

namespace Olbrasoft.Travel.EAN.Import
{
    public abstract class Importer : IImporter
    {
        protected readonly IProvider Provider;
        protected readonly IFactoryOfRepositories FactoryOfRepositories;
        protected readonly int DefaultLanguageId;
        protected readonly int CreatorId;
        protected readonly ILoggingImports Logger;

        private int _countRowsReaded;

        protected Importer(IProvider provider, IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger)
        {
            Provider = provider;
            FactoryOfRepositories = factoryOfRepositories;
            Logger = logger;
            DefaultLanguageId = sharedProperties.DefaultLanguageId;
            CreatorId = sharedProperties.CreatorId;
  
        }

        protected void Provider_SplittingLine(object sender, string[] items)
        {
            _countRowsReaded++;
            RowLoaded(items);
        }

        protected abstract void RowLoaded(string[] items);

        protected void LoadData(string path)
        {
            WriteLog("Load data from: " + path);
            Provider.SplittingLine += Provider_SplittingLine;
            Provider.ReadToEnd(path);
            Provider.SplittingLine -= Provider_SplittingLine;
            WriteLog(_countRowsReaded.ToString());
        }

        public abstract void Import(string path);
      
        
        protected static string ParsePath(string url)
        {
            url = url.Replace("https://i.travelapi.com/hotels/", "").Replace(Path.GetFileName(url), "");
            return url.Remove(url.Length - 1);
        }

        protected static string RebuildFileName(string url)
        {
            url = Path.GetFileNameWithoutExtension(url);

            if (url != null) return url.Remove(url.Length - 2) + "_b";

            throw new NullReferenceException();
        }

        protected static string GetSubClassName(string name)
        {
            return string.IsNullOrEmpty(name) ? null : name.ToLower().Replace("musuems", "museums");
        }

        protected void WriteLog(object obj)
        {
            Logger?.Log(obj.ToString());
        }

        protected void LogBuilded(int count)
        {
            WriteLog(count);
        }

        protected void LogBuild<TL>()
        {
            WriteLog($"{typeof(TL)} Build.");
        }

        protected void LogSave<TL>()
        {
            WriteLog($"{typeof(TL)} Save.");
        }

        protected void LogSaved<TL>()
        {
            WriteLog($"{typeof(TL)} Saved.");
        }
        

        public virtual void Dispose()
        {
            Provider.SplittingLine -= Provider_SplittingLine;
          GC.SuppressFinalize(this);
        }
    }
}