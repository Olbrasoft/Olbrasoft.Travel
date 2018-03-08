using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olbrasoft.Travel.BLL;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{
    abstract class Importer<T>:IImport where T : class, new()
    {
        protected readonly ImportOption Option;
        protected  ILoggingImports Logger;
        protected int BatchSize;
        protected IParserFactory ParserFactory;
        protected IParser<T> Parser;
        protected int CreatorId;
        protected int DefaultLanguageId;
        protected IImportProvider Provider;
        protected ILocalizedFacade LocalizedFacade;

        protected int BatchCount;

        protected Importer(ImportOption option)
        {
            Option = option ?? throw new ArgumentNullException(nameof(option));
        }
        
        public virtual void Import(string path)
        {
            Logger = Option.Logger;
            BatchSize = Option.ImportBatchSize;
            Provider = Option.Provider;
            CreatorId = Option.CreatorId;
            DefaultLanguageId = Option.DefaultLanguageId;
            LocalizedFacade = Option.LocalizedFacade;

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
            
            for (var batch = 1; batch <= BatchCount; batch++)
            {
                var lines = Provider.GetBatchLines(path, batch, BatchSize);
                var entities = Parser.Parse(lines);
                ImportBatch(entities.ToArray());
            }
        }
        
        protected abstract void ImportBatch(T[] parentRegions);
       
        public static int GetBatchCount(int countLines, int batchSize = 90000, bool skipFirstLine = true)
        {
            if (skipFirstLine) countLines = countLines - 1;
            return (int)Math.Ceiling(countLines / (double)batchSize);
        }

        public static DbGeography CreatePoint(double latitude, double longitude)
        {
            var point = string.Format(CultureInfo.InvariantCulture.NumberFormat,
                "POINT({0} {1})", longitude, latitude);
            // 4326 is most common coordinate system used by GPS/Maps
            return DbGeography.PointFromText(point, 4326);
        }

        public static DbGeography CreatePoligon(string s)
        {
            var spl = s.Split(':');
            var pointsString = new StringBuilder();

            string lastPoint = null;

            foreach (var s1 in spl)
            {
                var latLon = s1.Split(';');
                var lotLanString = $"{latLon[1]} {latLon[0]}";
                pointsString.Append(lotLanString + ",");
                if (string.IsNullOrEmpty(lastPoint)) lastPoint = lotLanString;
            }

            pointsString.Append(lastPoint);

            return DbGeography.PolygonFromText($"POLYGON(({pointsString}))", 4326);

        }

        protected void WriteLog(object obj)
        {
            Logger?.Log(obj.ToString());
        }

    }



}
