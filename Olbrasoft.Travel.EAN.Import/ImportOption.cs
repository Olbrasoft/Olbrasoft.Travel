using Olbrasoft.Travel.DAL;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class ImportOption
    {
        public int ImportBatchSize { get; set; }
        public ILoggingImports Logger { get; set; }
        public IParserFactory ParserFactory { get; set; }
        public int CreatorId { get; set; }
        public int DefaultLanguageId { get; set; }
        public IImportProvider Provider { get; set; }
        public readonly IFactoryOfRepositories FactoryOfRepositories;


        public ImportOption(IParserFactory parserFactory,  int creatorId, int defaultLanguageId, ILoggingImports logger , IFactoryOfRepositories factoryOfRepositories, int importBatchSize = 90000)
        {
            Logger = logger;
            FactoryOfRepositories = factoryOfRepositories;
            ImportBatchSize = importBatchSize;
            ParserFactory = parserFactory;
            CreatorId = creatorId;
            DefaultLanguageId = defaultLanguageId;
            
        }

    }
}
