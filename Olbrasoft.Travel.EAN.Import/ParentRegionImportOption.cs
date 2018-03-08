using Olbrasoft.Travel.BLL;
using Olbrasoft.Travel.DAL;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class ParentRegionImportOption : ImportOption
    {
        public readonly IContinentsRepository ContinentsRepository;
        public readonly ILocalizedContinentsRepository LocalizedContinentsRepository;
        public readonly ISubClassesRepository SubClassesRepository;
        public readonly IRegionsRepository RegionsRepository;

        public IRegionsFacade RegionsFacade { get; set; }
        public IPointsOfInterestFacade PointsOfInterestFacade { get; set; }
        public ISubClassesFacade SubClassesFacade { get; set; }


        
        public ParentRegionImportOption(IRegionsFacade regionsFacade, IPointsOfInterestFacade pointsOfInterestFacade, ISubClassesFacade subClassesFacade, IParserFactory parserFactory, ILocalizedFacade localizedFacade, int creatorId, int defaultLanguageId, ILoggingImports logger, IContinentsRepository continentsRepository, ILocalizedContinentsRepository localizedContinentsRepository, ISubClassesRepository subClassesRepository, IRegionsRepository regionsRepository, int importBatchSize = 90000) : base(parserFactory, localizedFacade, creatorId, defaultLanguageId, logger, importBatchSize)
        {
            SubClassesFacade = subClassesFacade;
            ContinentsRepository = continentsRepository;
            LocalizedContinentsRepository = localizedContinentsRepository;
            SubClassesRepository = subClassesRepository;
            RegionsRepository = regionsRepository;
            RegionsFacade = regionsFacade;
            PointsOfInterestFacade = pointsOfInterestFacade;
        }
        
      

    }

}