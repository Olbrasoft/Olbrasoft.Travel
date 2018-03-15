using System.Collections.Generic;
using System.Threading.Tasks;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;
using Country = Olbrasoft.Travel.EAN.DTO.Geography.Country;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class CountriesBatchImporter : BatchImporter<Country>
    {
       
        public CountriesBatchImporter(ImportOption option) : base(option)
        {
        }

        public override void ImportBatch(Country[] eanCountries)
        {
            var countriesRepository = FactoryOfRepositories.Geo<Travel.DTO.Country>();

            var continentsEanIdsToIds = FactoryOfRepositories.Geo<Continent>().EanIdsToIds;

            LogBuild<Travel.DTO.Country>();
            var countries = BuildCountries(eanCountries, continentsEanIdsToIds, countriesRepository, CreatorId);
            LogBuilded(countries.Length);

            LogSave<Travel.DTO.Country>();
            countriesRepository.BulkSave(countries);
            LogSaved<Travel.DTO.Country>();

            var eanRegionIdsToIds = countriesRepository.EanIdsToIds;

            LogBuild<LocalizedCountry>();
            var localizedCountries = BuildLocalizedCountries(eanCountries, eanRegionIdsToIds, DefaultLanguageId, CreatorId);
            LogBuilded(localizedCountries.Length);

            LogSave<LocalizedCountry>();
            FactoryOfRepositories.Localized<LocalizedCountry>().BulkSave(localizedCountries);
            LogSaved<LocalizedCountry>();

        }

        public  LocalizedCountry[] BuildLocalizedCountries(Country[] eanCountries,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds, int languageId, int creatorId)
        {
            
            //var localizedCountries = new Dictionary<int, LocalizedCountry>();
            //foreach (var eanCountry in eanCountries)
            //{
            //    if (!eanRegionIdsToIds.TryGetValue(eanCountry.CountryID, out var id)) continue;
            //    if (localizedCountries.ContainsKey(id)) continue;

            //    var localizedCountry = new LocalizedCountry
            //    {
            //        Id = id,
            //        LanguageId = languageId,
            //        Name = eanCountry.CountryName,
            //        CreatorId = creatorId
            //    };

            //    localizedCountries.Add(id, localizedCountry);
            //}

            //return localizedCountries.Values.ToArray();
            
            var localizedCountries = new Queue<LocalizedCountry>();
            Parallel.ForEach(eanCountries, eanCountry =>
            {
                if (!eanRegionIdsToIds.TryGetValue(eanCountry.CountryID, out var id)) return;
                var localizedCountry = new LocalizedCountry
                {
                    Id = id,
                    LanguageId = languageId,
                    Name = eanCountry.CountryName,
                    CreatorId = creatorId
                };

                lock (LockMe)
                {
                    localizedCountries.Enqueue(localizedCountry);
                }
            });

            return localizedCountries.ToArray();
        }

        public Travel.DTO.Country[] BuildCountries(Country[] eanCountries, 
            IReadOnlyDictionary<long, int> continentsEanIdsToIds,
            IMapToPartnersRepository<Travel.DTO.Country,long> repository,
            int creatorId)
        {

            //var countries = new Dictionary<long, Travel.DTO.Country>();
            //foreach (var eanCountry in eanCountries)
            //{
            //    if(!continentsEanIdsToIds.TryGetValue(eanCountry.ContinentID,out var continentId)) continue;
            //    if(countries.ContainsKey(eanCountry.CountryID)) continue;

            //    var country = new Travel.DTO.Country
            //    {
            //        ContinentId = continentId,
            //        EanId = eanCountry.CountryID,
            //        Code = eanCountry.CountryCode,
            //        CreatorId = creatorId
            //    };


            //    countries.Add(eanCountry.CountryID,country);
            //}
            //return countries.Values.ToArray();
            

            var countries = new Queue<Travel.DTO.Country>();
            Parallel.ForEach(eanCountries, eanCountry =>
            {
                if (!continentsEanIdsToIds.TryGetValue(eanCountry.ContinentID, out var continentId)) return;
                var country = new Travel.DTO.Country
                {
                    ContinentId = continentId,
                    EanId = eanCountry.CountryID,
                    Code = eanCountry.CountryCode,
                    CreatorId = creatorId
                };

                lock (LockMe)
                {
                    countries.Enqueue(country);
                }
            });

            return countries.ToArray();
        }
    }
}