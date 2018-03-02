using System;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public class LocalizedFacade : ILocalizedFacade
    {
        protected readonly ILocalizedRegionsRepository LocalizedRegionsRepository;
        protected readonly ILocalizedPointsOfInterestRepository LocalizedPointsOfInterestRepository;

        public LocalizedFacade(ILocalizedRegionsRepository localizedRegionsRepository, ILocalizedPointsOfInterestRepository localizedPointsOfInterestRepository)
        {
            LocalizedRegionsRepository = localizedRegionsRepository;
            LocalizedPointsOfInterestRepository = localizedPointsOfInterestRepository;
        }

        public bool Exists<T>(Language language)
        {
            if (typeof(T) == typeof(LocalizedRegion))
                return LocalizedRegionsRepository
                    .Exists(localizedRegion => localizedRegion.LanguageId == language.Id);

            if (typeof(T) == typeof(LocalizedPointOfInterest))
                return LocalizedPointsOfInterestRepository
                    .Exists(localizedPointOfInterest => localizedPointOfInterest.LanguageId == language.Id);
            
            throw new ArgumentException(nameof(T));
        }
    }
}