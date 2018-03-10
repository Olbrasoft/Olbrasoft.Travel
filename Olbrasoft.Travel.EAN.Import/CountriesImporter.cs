using System;
using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;
using Country = Olbrasoft.Travel.EAN.DTO.Geography.Country;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class CountriesImporter : Importer<Country>
    {
        public CountriesImporter(ImportOption option) : base(option)
        {
        }

        protected override void ImportBatch(Country[] eanCountries)
        {
            var continentsEanRegionIdsToIds = FactoryOfRepositories.BaseRegions<Continent>().EanRegionIdsToIds;
            var countriesRepository = FactoryOfRepositories.BaseRegions<Travel.DTO.Country>();

            var typeName = typeof(Travel.DTO.Country).Name;
            Logger.Log($"{typeName} Build.");
            var countries =  BuildCountries (eanCountries, continentsEanRegionIdsToIds ,countriesRepository, CreatorId);
            Logger.Log($"{typeName} Builded to insert:{countries.Count(c => c.Id==0)} to update:{countries.Count(c => c.Id != 0)}.");

            Logger.Log($"{typeName} Save.");
            countriesRepository.BulkSave(countries);
            Logger.Log($"{typeName} Saved.");

            var eanRegionIdsToIds = countriesRepository.EanRegionIdsToIds;

            Logger.Log($"{typeName} Build.");
            var localizedCountries = BuildLocalizedCountries(eanCountries, eanRegionIdsToIds, DefaultLanguageId, CreatorId);
            Logger.Log($"{typeName} Builded to insert:{localizedCountries.Count(c => c.Id == 0)} to update:{localizedCountries.Count(c => c.Id != 0)}.");

            Logger.Log($"{typeName} Save.");
            FactoryOfRepositories.Localized<LocalizedCountry>().BulkSave(localizedCountries);
            Logger.Log($"{typeName} Saved");


            //BulkSaveLocalized(localizedCountries, FactoryOfRepositories.Localized<LocalizedCountry>(),
            //    DefaultLanguageId, Logger);
        }

        public static LocalizedCountry[] BuildLocalizedCountries(Country[] eanCountries,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds, int languageId, int creatorId)
        {
            var localizedCountries= new Dictionary<int,LocalizedCountry>();
            foreach (var eanCountry in eanCountries)
            {
                if(!eanRegionIdsToIds.TryGetValue(eanCountry.CountryID,out var id)) continue;
                if(localizedCountries.ContainsKey(id)) continue;

                var localizedCountry=new LocalizedCountry
                {
                    Id = id,
                    LanguageId = languageId,
                    Name = eanCountry.CountryName,
                    CreatorId = creatorId
                };

                localizedCountries.Add(id,localizedCountry);
            }

            return localizedCountries.Values.ToArray();
        }

        public static  Travel.DTO.Country[] BuildCountries (Country[] eanCountries, IReadOnlyDictionary<long,int> continentsEanRegionIdsToIds ,IBaseRegionsRepository<Travel.DTO.Country> repository, int creatorId)
        {
            var eanRegionIdsToIds = repository.EanRegionIdsToIds;

            var countries = new Dictionary<long, Travel.DTO.Country>();

            foreach (var eanCountry in eanCountries)
            {
                if(!continentsEanRegionIdsToIds.TryGetValue(eanCountry.ContinentID,out var continentId)) continue;
                if(countries.ContainsKey(eanCountry.CountryID)) continue;

                var country = new Travel.DTO.Country
                {
                    ContinentId = continentId,
                    EanRegionId = eanCountry.CountryID,
                    Code = eanCountry.CountryCode
                    
                };

                if (eanRegionIdsToIds.TryGetValue(eanCountry.CountryID, out var id))
                {
                    country.Id = id;
                }
                else
                {
                    country.CreatorId = creatorId;
                }
               
                countries.Add(eanCountry.CountryID,country);
            }

            return countries.Values.ToArray();
        }
    }
}