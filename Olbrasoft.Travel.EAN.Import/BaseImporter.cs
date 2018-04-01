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


        protected static string GetSubClassName(string name)
        {
            return string.IsNullOrEmpty(name) ? null : name.ToLower().Replace("musuems", "museums");
        }

        protected IParser<T> CreateParser(string path)
        {
            if (Option.ParserFactory == null)
            {
                WriteLog($"{nameof(Option.ParserFactory)} is Null {typeof(T)} import will be terminated.");
                throw new Exception("Create parser error!");
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

        protected static string ParsePath(string url)
        {
            url = url.Replace("https://i.travelapi.com/hotels/", "").Replace(System.IO.Path.GetFileName(url), "");
            return url.Remove(url.Length - 1);
        }
        

        protected IReadOnlyDictionary<string, int> ImportPathsToPhotos(IEnumerable<string> urls,
            IPathsToPhotosRepository repository, int creatorId)
        {
            LogBuild<PathToPhoto>();
            var pathsToPhotos = BuildPathsToPhotos(urls, repository.Paths, creatorId);
            var count = pathsToPhotos.Length;
            LogBuilded(count);

            if (count <= 0) return repository.PathsToIds;

            LogSave<PathToPhoto>();
            repository.BulkSave(pathsToPhotos);
            LogSaved<PathToPhoto>();

            return repository.PathsToIds;
        }
        

        private static PathToPhoto[] BuildPathsToPhotos(IEnumerable<string> urls, ICollection<string> paths, int creatorId)
        {
            var group = urls.Select(ParsePath).Distinct();

            return group.Where(ptp => !paths.Contains(ptp)).Select(ptp => new PathToPhoto()
            {
                Path = ptp,
                CreatorId = creatorId

            }).ToArray();
        }
        

        protected IReadOnlyDictionary<string, int> ImportFilesExtensions(IEnumerable<string> urls, IFilesExtensionsRepository repository, int creatorId)
        {
            LogBuild<FileExtension>();
            var filesExtensions = BuildFilesExtensions(urls, repository.Extensions, creatorId);
            var count = filesExtensions.Length;
            LogBuilded(count);

            if (count <= 0) return repository.ExtensionsToIds;

            LogSave<FileExtension>();
            repository.Save(filesExtensions);
            LogSaved<FileExtension>();

            return repository.ExtensionsToIds;
        }


        private static FileExtension[] BuildFilesExtensions(IEnumerable<string> urls, ICollection<string> storedExtensions, int creatorId)
        {
            return urls.Select(url => System.IO.Path.GetExtension(url)?.ToLower()).Where(e => !storedExtensions.Contains(e)).Distinct()
                .Select(p => new FileExtension { Extension = p, CreatorId = creatorId }).ToArray();
        }


        protected LocalizedRegion[] BuildLocalizedRegions(IEnumerable<IHaveRegionIdRegionNameRegionNameLong> eanEntities,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds,
            int langiageId,
            int creatorId
        )
        {
            var localizedRegions = new Queue<LocalizedRegion>();

            Parallel.ForEach(eanEntities, eL =>
            {
                if (!eanRegionIdsToIds.TryGetValue(eL.RegionID, out var id)) return;

                var localizedRegion = new LocalizedRegion
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


        protected Region[] BuildRegions(IEnumerable<CityNeighborhood> eanCitiesOrNeighborhoods, int creatorId)
        {
            var regions = new Queue<Region>();

            Parallel.ForEach(eanCitiesOrNeighborhoods, eanCityOrNeighborhood =>
            {
                var region = new Region
                {
                    EanId = eanCityOrNeighborhood.RegionID,
                    Coordinates = CreatePoligon(eanCityOrNeighborhood.Coordinates),
                    CreatorId = creatorId
                };

                lock (LockMe)
                {
                    regions.Enqueue(region);
                }
            });

            return regions.ToArray();
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