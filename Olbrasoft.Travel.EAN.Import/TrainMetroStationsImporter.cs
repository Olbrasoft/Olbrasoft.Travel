using System.Collections.Generic;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{




    internal class TrainMetroStationsImporter : Importer
    {
        private readonly IParserFactory _parserFactory;
        protected IParser<TrainMetroStationCoordinates> Parser;
        protected Queue<TrainMetroStationCoordinates> TrainMetroStations = new Queue<TrainMetroStationCoordinates>();
        

        public TrainMetroStationsImporter(IProvider provider, IParserFactory parserFactory ,IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger) 
            : base(provider, factoryOfRepositories, sharedProperties, logger)
        {
            _parserFactory = parserFactory;
        }

        protected override void RowLoaded(string[] items)
        {
            TrainMetroStations.Enqueue(Parser.Parse(items));
        }

        public override void Import(string path)
        {
            Parser = _parserFactory.Create<TrainMetroStationCoordinates>(Provider.GetFirstLine(path));

            LoadData(path);
           
        }
        
    }




}