using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olbrasoft.Travel.BLL;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{
    internal abstract class Importer<T>:IImport<T> where T : class, new()
    {
        private readonly object _lockMe = new object();
        protected readonly ImportOption Option;
        protected  ILoggingImports Logger;
        protected int BatchSize;
        protected IParserFactory ParserFactory;
        protected IParser<T> Parser;
        protected int CreatorId;
        protected int DefaultLanguageId;
        protected IImportProvider Provider;
        protected IFactoryOfRepositories FactoryOfRepositories;

        protected int BatchCount;

        protected Importer(ImportOption option)
        {
            Option = option ?? throw new ArgumentNullException(nameof(option));
        }

        public static IEnumerable<List<T>> SplitList<T>(IEnumerable<T> locations, int nSize = 90000)
        {
            var result = locations.ToList();
            for (var i = 0; i < result.Count; i += nSize)
            {
                yield return result.GetRange(i, Math.Min(nSize, result.Count - i));
            }
        }

        public virtual void Import(string path)
        {
            Logger = Option.Logger;
            BatchSize = Option.ImportBatchSize;
            Provider = Option.Provider;
            CreatorId = Option.CreatorId;
            DefaultLanguageId = Option.DefaultLanguageId;
            FactoryOfRepositories = Option.FactoryOfRepositories;

            var countLines = Provider.GetCountLines(path);
            BatchCount = GetBatchCount(countLines, BatchSize);
            WriteLog($"Start Import {typeof(T)} from:{path} import batch size is:{BatchSize} count lines is:{countLines} bacth count is:{BatchCount}.");

            if (Option.ParserFactory == null)
            {
                WriteLog($"{nameof(Option.ParserFactory)} is Null {typeof(T)} import will be terminated.");
                return;
            }

            ParserFactory = Option.ParserFactory;
            Parser = ParserFactory.Create<T>(Provider.GetFirstLine(path));

            //var listLines = SplitList( File.ReadAllLines(path).Skip(1));
            //foreach (var lines in listLines)
            //{
            //    var entities = Parser.Parse(lines);
            //    ImportBatch(entities.ToArray());
            //}


            for (var batch = 1; batch <= BatchCount; batch++)
            {
                var lines = Provider.GetBatchLines(path, batch, BatchSize);
                var entities = Parser.Parse(lines);
                ImportBatch(entities.ToArray());
            }
        }

        public abstract void ImportBatch(T[] parentRegions);
       
        public static int GetBatchCount(int countLines, int batchSize = 90000, bool skipFirstLine = true)
        {
            if (skipFirstLine) countLines = countLines - 1;
            return (int)Math.Ceiling(countLines / (double)batchSize);
        }

        public static DbGeography CreatePoint(double latitude, double longitude)
        {
            var point = String.Format(CultureInfo.InvariantCulture.NumberFormat,
                "POINT({0} {1})", longitude, latitude);
            // 4326 is most common coordinate system used by GPS/Maps
            return DbGeography.PointFromText(point, 4326);
        }
        
        public  DbGeography CreatePoligon(string s)
        {
            var spl = s.Split(':');
            var pointsString = new StringBuilder();
            
            foreach (var s1 in spl)
            {
                var latLon = s1.Split(';');
               // var lotLanString = $"{latLon[1]} {latLon[0]},";
                pointsString.Append($"{latLon[1]} {latLon[0]},");
            }

            pointsString.Append(pointsString.ToString().Split(',').First());

            return DbGeography.PolygonFromText($"POLYGON(({pointsString}))", 4326);
        }


        protected TCn[] BuildCitiesOrNeighborhoods<TCn>(IEnumerable<CityNeighborhood> eanCities, int creatorId) where TCn:BaseRegionCoordinates,new()
        {
            var cities = new Queue<TCn>();
            Parallel.ForEach(eanCities, eanCity =>
            {
                var city = new TCn
                {
                    EanRegionId = eanCity.RegionID,
                    Coordinates = CreatePoligon(eanCity.Coordinates),
                    CreatorId = creatorId
                };

                lock (_lockMe)
                {
                    cities.Enqueue(city);
                }

            });
            
            return cities.ToArray();
        }



        //protected static void BulkSaveLocalized<TL>(IReadOnlyCollection<TL> localizedEntities,
        //      ILocalizedRepository<TL> repository, int defaultLanguageId, ILoggingImports logger) where TL : BaseLocalized
        //  {
        //      if (!repository.Exists(defaultLanguageId))
        //      {
        //          logger.Log($"Bulk Insert {localizedEntities.Count} {typeof(TL)}.");
        //          repository.BulkInsert(localizedEntities);
        //          logger.Log($"{typeof(TL)} Inserted.");
        //      }
        //      else
        //      {
        //          var storedLocalizedIds =
        //              new HashSet<int>(repository.FindIds(defaultLanguageId));

        //          var forInsert = new List<TL>();
        //          var forUpdate = new List<TL>();

        //          foreach (var localizedEntity in localizedEntities)
        //          {
        //              if (!storedLocalizedIds.Contains(localizedEntity.Id))
        //              {
        //                  forInsert.Add(localizedEntity);
        //              }
        //              else
        //              {
        //                  forUpdate.Add(localizedEntity);
        //              }
        //          }

        //          if (forInsert.Count > 0)
        //          {
        //              logger.Log($"Bulk Insert {forInsert.Count} {typeof(TL)}.");
        //              repository.BulkInsert(forInsert);
        //              logger.Log($"{typeof(TL)} Inserted.");
        //          }

        //          if (forUpdate.Count <= 0) return;
        //          logger.Log($"Bulk Update {forUpdate.Count} {typeof(TL)}.");
        //          repository.BulkUpdate(forUpdate);
        //          logger.Log($"{typeof(TL)} Updated.");
        //      }
        //  }

        protected void WriteLog(object obj)
        {
            Logger?.Log(obj.ToString());
        }
    }



}
