﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Olbrasoft.Travel.Data.Entity;
using Olbrasoft.Travel.DataAccessLayer;
using Olbrasoft.Travel.ExpediaAffiliateNetwork.DataTransferObject.Geography;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.Import
{
    internal abstract class Importer<T> : Importer where T : class, new()
    {
        protected readonly object LockMe = new object();
        private readonly IParserFactory _parserFactory;
        protected IParser<T> Parser;
        protected Queue<T> EanDataTransferObjects = new Queue<T>();
        
        protected Importer(IProvider provider, IParserFactory parserFactory, IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger)
            : base(provider, factoryOfRepositories, sharedProperties, logger)
        {
            _parserFactory = parserFactory;
        }

        protected override void RowLoaded(string[] items)
        {
            EanDataTransferObjects.Enqueue(Parser.Parse(items));
        }

        protected override void LoadData(string path)
        {
            Parser = _parserFactory.Create<T>(Provider.GetFirstLine(path));
            base.LoadData(path);
        }
        
        protected void ImportLocalizedRegions(
            IEnumerable<IHaveRegionIdRegionName> eanDataTransferObjects,
            ILocalizedRepository<LocalizedRegion> repository,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            int languageId,
            int creatorId

        )
        {
            LogBuild<LocalizedRegion>();
            var localizedRegions = BuildLocalizedRegions(eanDataTransferObjects, eanIdsToIds, languageId, creatorId);
            var count = localizedRegions.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<LocalizedRegion>();
            repository.BulkSave(localizedRegions, count);
            LogSaved<LocalizedRegion>();
        }


        protected void ImportLocalizedAccommodations(
            IEnumerable<IToLocalizedAccommodation> eanDataTransferObjects,
            ILocalizedRepository<LocalizedAccommodation> repository,
            IReadOnlyDictionary<int, int> accommodationsEanIdsToIds,
            int languageId,
            int creatorId
        )
        {
            LogBuild<LocalizedAccommodation>();
            var localizedAccommodations = BuildLocalizedAccommodations(eanDataTransferObjects,
                accommodationsEanIdsToIds, languageId, creatorId);

            var count = localizedAccommodations.Length;

            LogBuilded(count);

            if (count <= 0) return;

            LogSave<LocalizedAccommodation>();
            repository.BulkSave(localizedAccommodations);
            LogSaved<LocalizedAccommodation>();
        }

        protected static LocalizedAccommodation[] BuildLocalizedAccommodations(
            IEnumerable<IToLocalizedAccommodation> eanEntities,
            IReadOnlyDictionary<int, int> eanIdsToIds,
            int languageId,
            int creatorId
        )
        {
            var localizedAccommodations = new Queue<LocalizedAccommodation>();

            foreach (var activeProperty in eanEntities)
            {
                if (!eanIdsToIds.TryGetValue(activeProperty.EANHotelID, out var id)) continue;

                var localizedAccommodation = new LocalizedAccommodation()
                {
                    Id = id,
                    LanguageId = languageId,
                    Name = activeProperty.Name,
                    Location = activeProperty.Location,
                    CheckInTime = activeProperty.CheckInTime,
                    CheckOutTime = activeProperty.CheckOutTime,
                    CreatorId = creatorId
                };

                localizedAccommodations.Enqueue(localizedAccommodation);
            }

            return localizedAccommodations.ToArray();
        }

        protected CultureInfo CultureInfo( string eanLanguageCode )
        {
            return  new CultureInfo(eanLanguageCode.Replace("_","-"));
        }

        protected string EanLanguageCode(string fileName)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

            return fileNameWithoutExtension?.Substring(fileNameWithoutExtension.Length - 5);
        }

        protected LocalizedRegion[] BuildLocalizedRegions(IEnumerable<IHaveRegionIdRegionName> eanEntities,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            int languageId,
            int creatorId
        )
        {
            var localizedRegions = new Queue<LocalizedRegion>();
            Parallel.ForEach(eanEntities, eanEntity =>
            {
                if (!eanIdsToIds.TryGetValue(eanEntity.RegionID, out var id)) return;

                var localizedRegion = new LocalizedRegion()
                {
                    Id = id,
                    LanguageId = languageId,
                    CreatorId = creatorId,
                    Name = eanEntity.RegionName
                };

                lock (LockMe)
                {
                    localizedRegions.Enqueue(localizedRegion);
                }
            });

            return localizedRegions.ToArray();
        }


        protected Region[] BuildRegions(IEnumerable<IHaveRegionIdLatitudeLongitude> entities,
            int cretorId
        )
        {
            var regions = new Queue<Region>();

            //foreach (var entity in entities)
            //{
            //    var region = new Region
            //    {
            //        EanId = entity.RegionID,
            //        CenterCoordinates = CreatePoint(entity.Latitude, entity.Longitude),
            //        CreatorId = cretorId
            //    };

            //     regions.Enqueue(region);

            //}

            Parallel.ForEach(entities, entity =>
            {
                var region = new Region
                {
                    EanId = entity.RegionID,
                    CenterCoordinates = CreatePoint(entity.Latitude, entity.Longitude),
                    CreatorId = cretorId
                };

                lock (LockMe)
                {
                    regions.Enqueue(region);
                }
            });

            return regions.ToArray();
        }
        
        protected RegionToType[] BuildRegionsToTypes(IEnumerable<IHaveRegionId> eanEntities,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            int typeOfRegionId,
            int subClassId,
            int creatorId
        )
        {
            var regionsToTypes = new Queue<RegionToType>();
            Parallel.ForEach(eanEntities, eanEntity =>
            {
                if (!eanIdsToIds.TryGetValue(eanEntity.RegionID, out var id)) return;

                var regionToType = new RegionToType
                {
                    Id = id,
                    ToId = typeOfRegionId,
                    SubClassId = subClassId,
                    CreatorId = creatorId
                };

                lock (LockMe)
                {
                    regionsToTypes.Enqueue(regionToType);
                }

            });

            return regionsToTypes.ToArray();
        }

    }

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

        protected virtual void LoadData(string path)
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