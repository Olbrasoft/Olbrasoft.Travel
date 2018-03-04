using Olbrasoft.Travel.BLL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.EAN.Import
{
    class ParentRegionImportOption : ImportOption
    {

        public IRegionsFacade RegionsFacade { get; set; }
        public IPointsOfInterestFacade PointsOfInterestFacade { get; set; }
        public ISubClassesFacade SubClassesFacade { get; set; }
        
        public ParentRegionImportOption(IRegionsFacade regionsFacade, IPointsOfInterestFacade pointsOfInterestFacade, ISubClassesFacade subClassesFacade, IParserFactory parserFactory, ILocalizedFacade localizedFacade, int creatorId, int defaultLanguageId, ILoggingImports logger, int importBatchSize = 90000) : base(parserFactory, localizedFacade, creatorId, defaultLanguageId, logger, importBatchSize)
        {
            SubClassesFacade = subClassesFacade;
            RegionsFacade = regionsFacade;
            PointsOfInterestFacade = pointsOfInterestFacade;
        }



      

    }

}