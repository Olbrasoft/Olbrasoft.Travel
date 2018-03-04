using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olbrasoft.Travel.BLL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.EAN.Import
{
    class ImportOption
    {
        public int ImportBatchSize { get; set; }
        public ILoggingImports Logger { get; set; }
        public IParserFactory ParserFactory { get; set; }
        public int CreatorId { get; set; }
        public int DefaultLanguageId { get; set; }
        public IImportProvider Provider { get; set; }
        public ILocalizedFacade LocalizedFacade { get; set; }


        public ImportOption(IParserFactory parserFactory, ILocalizedFacade localizedFacade , int creatorId, int defaultLanguageId, ILoggingImports logger , int importBatchSize = 90000)
        {
            Logger = logger;
            ImportBatchSize = importBatchSize;
            ParserFactory = parserFactory;
            CreatorId = creatorId;
            DefaultLanguageId = defaultLanguageId;
            LocalizedFacade = localizedFacade;
        }

    }
}
