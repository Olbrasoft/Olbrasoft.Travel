using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        protected  TLr[] BuildLocalizedRegions<TLr>(IEnumerable<IHaveRegionIdRegionNameRegionNameLong> eanEntities,
            IReadOnlyDictionary<long,int> eanRegionIdsToIds,
            int langiageId,
            int creatorId
        ) where TLr : LocalizedRegionWithNameAndLongName, new()
        {
            var localizedRegions= new Queue<TLr>();

            Parallel.ForEach(eanEntities, eL =>
            {
                if(!eanRegionIdsToIds.TryGetValue(eL.RegionID, out var id)) return;

                var localizedRegion = new TLr
                {
                    Id = id,
                    LanguageId = langiageId,
                    Name = eL.RegionName
                };

                if (eL.RegionName != eL.RegionNameLong)
                    localizedRegion.LongName = eL.RegionNameLong;

                lock (_lockMe)
                {
                    localizedRegions.Enqueue(localizedRegion);
                }
            });

            return localizedRegions.ToArray();
        }

        protected TCn[] BuildCitiesOrNeighborhoods<TCn>(IEnumerable<CityNeighborhood> eanCities, int creatorId) where TCn:BaseRegionWithCoordinates,new()
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
        
        protected TLe[] BuildLocalizedCitiesOrNeighborhoods<TLe>(
            IEnumerable<CityNeighborhood> eanCities, 
            IReadOnlyDictionary<long, int> eanRegionIdsToIds, 
            int languageId, 
            int creatorId
            ) where TLe: LocalizedRegionWithNameAndLongName,new()
        {
            var localizedCities = new Queue<TLe>();

            Parallel.ForEach(eanCities, eanCity =>
            {
                if (!eanRegionIdsToIds.TryGetValue(eanCity.RegionID, out var id)) return;
                
                var localizedCity = new TLe
                {
                    Id = id,
                    LanguageId = languageId,
                    Name = eanCity.RegionName,
                    CreatorId = creatorId
                };

                lock (_lockMe)
                {
                    localizedCities.Enqueue(localizedCity);
                }
            });
            return localizedCities.ToArray();
        }


        protected void LogBuilded(int count)
        {
            WriteLog(count);
        }

        protected void LogBuild<TL>()
        {
            WriteLog($"{typeof(TL)} Build.");
        }

        protected void LogSave<TL>( )
        {
            WriteLog($"{typeof(TL)} Save.");
        }
        protected void LogSaved<TL>()
        {
            WriteLog($"{typeof(TL)} Saved.");
        }

        protected void WriteLog(object obj)
        {
            Logger?.Log(obj.ToString());
        }
    }



}
