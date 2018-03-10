using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        public bool Exists<T>(int languageId)
        {
            if (typeof(T) == typeof(LocalizedRegion))
                return LocalizedRegionsRepository
                    .Exists(localizedRegion => localizedRegion.LanguageId == languageId);

            if (typeof(T) == typeof(LocalizedPointOfInterest))
                return LocalizedPointsOfInterestRepository
                    .Exists(localizedPointOfInterest => localizedPointOfInterest.LanguageId == languageId);
            
            throw new ArgumentException(nameof(T));
        }

        public static bool IsValid(IEnumerable<ILocalized> localizeds)
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(p => p.Parent.IsNeutralCulture && p.Name.Length == 5);
            var hashCulturesId = new HashSet<int>(cultures.Select(p => p.LCID));

            return localizeds.All(localized => hashCulturesId.Contains(localized.LanguageId));
        }

        public static bool TryGetOneLanguageId(IEnumerable<ILocalized> localizeds, out int cultureId)
        {
            cultureId = 0;
            var localizedsArray = localizeds as ILocalized[] ?? localizeds.ToArray();
            if (!IsValid(localizedsArray)) return false;

            var g = localizedsArray.GroupBy(p => p.LanguageId);

            var gArray = g as IGrouping<int, ILocalized>[] ?? g.ToArray();
            if (gArray.Length!= 1) return false;

            var first = gArray.FirstOrDefault();

            if (first != null) cultureId = first.Key;
            else
            {
                return false;
            }

            return true;
        }

        //public void BulkSave(LocalizedRegion[] localizedRegions)
        //{
        //    if (TryGetOneLanguageId(localizedRegions, out var languageId))
        //    {
        //        if (!Exists<LocalizedRegion>(languageId))
        //        {
        //            LocalizedRegionsRepository.BulkInsert(localizedRegions);
        //        }
        //        else
        //        {
        //            LocalizedRegionsRepository.BulkInsertOrUpdate(localizedRegions);
        //        }
        //    }
        //    else
        //    {
        //        LocalizedRegionsRepository.BulkInsertOrUpdate(localizedRegions);
        //    }
        //}

        //public void BulkSave(LocalizedPointOfInterest[] localizedPointsOfInterest)
        //{
        //    if (TryGetOneLanguageId(localizedPointsOfInterest, out var languageId))
        //    {
        //        if (!Exists<LocalizedPointOfInterest>(languageId))
        //        {
        //            LocalizedPointsOfInterestRepository.BulkInsert(localizedPointsOfInterest);
        //        }
        //        else
        //        {
        //            LocalizedPointsOfInterestRepository.BulkInsertOrUpdate(localizedPointsOfInterest);
        //        }
        //    }
        //    else
        //    {
        //        LocalizedPointsOfInterestRepository.BulkInsertOrUpdate(localizedPointsOfInterest);
        //    }
        //}
    }
}