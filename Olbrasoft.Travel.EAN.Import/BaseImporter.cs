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
    internal abstract class BaseImporter<T> : IImport<T> where T : class, new()
    {
        protected readonly object LockMe = new object();
        protected readonly ImportOption Option;
        protected ILoggingImports Logger;
        protected IParserFactory ParserFactory;
        protected int CreatorId;
        protected int DefaultLanguageId;
        protected IImportProvider Provider;
        protected IFactoryOfRepositories FactoryOfRepositories;

        protected BaseImporter(ImportOption option)
        {
            Option = option ?? throw new ArgumentNullException(nameof(option));

            Logger = Option.Logger;
            Provider = Option.Provider;
            CreatorId = Option.CreatorId;
            DefaultLanguageId = Option.DefaultLanguageId;
            FactoryOfRepositories = Option.FactoryOfRepositories;
            
        }

        protected IParser<T> CreateParser(string path)
        {
            if (Option.ParserFactory == null)
            {
                WriteLog($"{nameof(Option.ParserFactory)} is Null {typeof(T)} import will be terminated.");
                throw   new Exception("Create parser error!");
            }

            ParserFactory = Option.ParserFactory;
          return ParserFactory.Create<T>(Provider.GetFirstLine(path));

        }


        public abstract void Import(string path);

        public static DbGeography CreatePoint(double latitude, double longitude)
        {
            var point = string.Format(CultureInfo.InvariantCulture.NumberFormat,
                "POINT({0} {1})", longitude, latitude);
            // 4326 is most common coordinate system used by GPS/Maps
            return DbGeography.PointFromText(point, 4326);
        }

        public DbGeography CreatePoligon(string s)
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



        protected TLr[] BuildLocalizedRegions<TLr>(IEnumerable<IHaveRegionIdRegionNameRegionNameLong> eanEntities,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds,
            int langiageId,
            int creatorId
        ) where TLr : LocalizedRegionWithNameAndLongName, new()
        {
            var localizedRegions = new Queue<TLr>();

            Parallel.ForEach(eanEntities, eL =>
            {
                if (!eanRegionIdsToIds.TryGetValue(eL.RegionID, out var id)) return;

                var localizedRegion = new TLr
                {
                    Id = id,
                    LanguageId = langiageId,
                    Name = eL.RegionName
                };

                if (eL.RegionName != eL.RegionNameLong)
                    localizedRegion.LongName = eL.RegionNameLong;

                lock (LockMe)
                {
                    localizedRegions.Enqueue(localizedRegion);
                }
            });

            return localizedRegions.ToArray();
        }

        protected TCn[] BuildCitiesOrNeighborhoods<TCn>(IEnumerable<CityNeighborhood> eanCities, int creatorId) where TCn : GeoWithCoordinates, new()
        {
            var cities = new Queue<TCn>();
            Parallel.ForEach(eanCities, eanCity =>
            {
                var city = new TCn
                {
                    EanId = eanCity.RegionID,
                    Coordinates = CreatePoligon(eanCity.Coordinates),
                    CreatorId = creatorId
                };

                lock (LockMe)
                {
                    cities.Enqueue(city);
                }

            });

            return cities.ToArray();
        }

        protected TLr[] BuildLocalizedRegions<TLr>(
            IEnumerable<CityNeighborhood> eanCities,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds,
            int languageId,
            int creatorId
        ) where TLr : BaseLocalizedRegion, new()
        {
            var localizedCities = new Queue<TLr>();

            Parallel.ForEach(eanCities, eanCity =>
            {
                if (!eanRegionIdsToIds.TryGetValue(eanCity.RegionID, out var id)) return;

                var localizedCity = new TLr
                {
                    Id = id,
                    LanguageId = languageId,
                    Name = eanCity.RegionName,
                    CreatorId = creatorId
                };

                lock (LockMe)
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

        protected void LogSave<TL>()
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