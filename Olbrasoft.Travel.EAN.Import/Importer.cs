using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DAL.EntityFramework.Migrations;
using Olbrasoft.Travel.DTO;

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

        
        protected IReadOnlyDictionary<long, int> ImportRegions(
            Region[] regions,
            IRegionsRepository repository,
            int batchSize,
            params Expression<Func<Region, object>>[] ignorePropertiesWhenUpdating
        )
        {
            if (regions.Length <= 0) return repository.EanIdsToIds;
            LogSave<Region>();
            repository.BulkSave(regions, batchSize, ignorePropertiesWhenUpdating);
            LogSaved<Region>();

            return repository.EanIdsToIds;
        }


        protected void ImportLocalizedRegions(IDictionary<long, Tuple<string, string>> adeptsToLocalizedRegions,
            ILocalizedRepository<LocalizedRegion> repository,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            int languageId,
            int creatorId
        )
        {
            LogBuild<LocalizedRegion>();
            var localizedRegions = BuildLocalizedRegions(adeptsToLocalizedRegions, eanIdsToIds, languageId, creatorId);
            var count = localizedRegions.Length;
            LogBuilded(count);

            if (count <= 0) return;

            LogSave<LocalizedRegion>();
            repository.BulkSave(localizedRegions, count, lr => lr.LongName);
            LogSaved<LocalizedRegion>();
        }


        protected LocalizedRegion[] BuildLocalizedRegions(
            IDictionary<long, Tuple<string, string>> adeptsToLocalizedRegions,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            int languageId,
            int creatorId
        )
        {
            var localizedRegions = new Queue<LocalizedRegion>();

            foreach (var adeptToLocalizedRegion in adeptsToLocalizedRegions)
            {
                if (!eanIdsToIds.TryGetValue(adeptToLocalizedRegion.Key, out var id)) continue;

                var localizedRegion = new LocalizedRegion()
                {
                    Id = id,
                    LanguageId = languageId,
                    CreatorId = creatorId,
                    Name = adeptToLocalizedRegion.Value.Item1
                };

                if (!string.IsNullOrEmpty(adeptToLocalizedRegion.Value.Item2))
                {
                    localizedRegion.LongName = adeptToLocalizedRegion.Value.Item2;
                }

                localizedRegions.Enqueue(localizedRegion);
            }

            return localizedRegions.ToArray();
        }

        public static DbGeography CreatePoint(double latitude, double longitude)
        {
            var point = string.Format(CultureInfo.InvariantCulture.NumberFormat,
                "POINT({0} {1})", longitude, latitude);
            // 4326 is most common coordinate system used by GPS/Maps
            return DbGeography.PointFromText(point, 4326);
        }

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