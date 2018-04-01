using System.Collections.Generic;
using Olbrasoft.Travel.DTO;
using Country = Olbrasoft.Travel.EAN.DTO.Geography.Country;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class CountriesBatchImporter : BatchImporter<Country>
    {
        public class CandidateCountry
        {
            public string Name { get; set; }
            public string Code { get; set; }
        }

        CandidateCountry[] ProbablyMissingCoutries => new[]
        {
            new CandidateCountry
            {
                Name = "Yemen",
                Code = "YE"
            },

            new CandidateCountry()
            {
                Name = "Somalia",
                Code = "SO"
            },
            new CandidateCountry()
            {
                Name = "Timor-Leste",
                Code = "TL"
            },
        };

        public CountriesBatchImporter(ImportOption option) : base(option)
        {
        }

        public override void ImportBatch(Country[] eanCountries)
        {

            var regionsRepository = FactoryOfRepositories.Regions();
            var eanIdsToIds = regionsRepository.EanIdsToIds;
            var typeOfRegionCountryId = FactoryOfRepositories.BaseNames<TypeOfRegion>().GetId("Country");

            Logger.Log("Build Regions from CountryList");
            var regions = BuildRegions(eanCountries, eanIdsToIds, CreatorId);
            var count = regions.Length;
            Logger.Log($"Builded {count} Regions from CountryList.");

            if (count > 0)
            {
                LogSave<Region>();
                regionsRepository.BulkSave(regions);
                LogSaved<Region>();

                eanIdsToIds = regionsRepository.EanIdsToIds;
            }
            

            Logger.Log("Build localizedRegions from CountryList");
            var localizedRegions = BuildLocalizedRegions(eanCountries, eanIdsToIds, DefaultLanguageId, CreatorId);
            count = localizedRegions.Length;
            LogBuilded(count);

            if (count > 0)
            {
                LogSave<LocalizedRegion>();
                FactoryOfRepositories.Localized<LocalizedRegion>().BulkSave(localizedRegions);
                LogSaved<LocalizedRegion>();
            }


            LogBuild<RegionToType>();
            var regionsToTypes = BuildRegionsToTypes(eanCountries, eanIdsToIds, typeOfRegionCountryId, CreatorId);
            count = regionsToTypes.Length;
            LogBuilded(count);

            if (count > 0)
            {
                LogSave<RegionToType>();
                FactoryOfRepositories.RegionsToTypes().BulkSave(regionsToTypes);
                LogSaved<RegionToType>();
            }
            
            LogBuild<Travel.DTO.Country>();
            var countries = BuildCountries(eanCountries, eanIdsToIds, CreatorId);
            LogBuilded(countries.Length);

            var countriesRepository = FactoryOfRepositories.AdditionalRegionsInfo<Travel.DTO.Country>();

            LogSave<Travel.DTO.Country>();
            countriesRepository.BulkSave(countries);
            LogSaved<Travel.DTO.Country>();

            Logger.Log($"Build RegionsToRegions from CoutryList");
            var regionsToRegions = BuildRegionsToRegions(eanCountries, eanIdsToIds, CreatorId);
            count = regionsToRegions.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<RegionToRegion>();
            FactoryOfRepositories.ManyToMany<RegionToRegion>().BulkSave(regionsToRegions);
            LogSaved<RegionToRegion>();


            foreach (var probablyMissingCoutry in ProbablyMissingCoutries)
            {
                if (countriesRepository.Exists(c => c.Code == probablyMissingCoutry.Code)) continue;

                var region = new Region
                {
                    CreatorId = CreatorId
                };

                region.LocalizedRegions.Add(new LocalizedRegion
                {
                    LanguageId = DefaultLanguageId,
                    Name = probablyMissingCoutry.Name,
                    CreatorId = CreatorId
                });

                region.RegionsToTypes.Add(new RegionToType
                {
                    ToId = typeOfRegionCountryId,
                    CreatorId = CreatorId
                });

                var country = new Travel.DTO.Country
                {
                    Code = probablyMissingCoutry.Code,
                    CreatorId = CreatorId
                };


                region.AdditionalCountryProperties = country;

                regionsRepository.Add(region);
            }

        }
        
        private static RegionToType[] BuildRegionsToTypes(IEnumerable<Country> eanCountries,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            int typeOfRegionCountryId,
            int creatorId
            )
        {

            var regionsToTypes = new Queue<RegionToType>();

            foreach (var eanCountry in eanCountries)
            {
                if (!eanIdsToIds.TryGetValue(eanCountry.CountryID, out var id)) continue;

                var regionToType = new RegionToType
                {
                    Id = id,
                    ToId = typeOfRegionCountryId,
                    CreatorId = creatorId
                };

                regionsToTypes.Enqueue(regionToType);
            }

            return regionsToTypes.ToArray();

        }

        

        private static RegionToRegion[] BuildRegionsToRegions(
            IEnumerable<Country> eanCountries,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            int creatorId
            )
        {
            var regionsToregions = new Queue<RegionToRegion>();
            foreach (var eanCountry in eanCountries)
            {
                if (!eanIdsToIds.TryGetValue(eanCountry.CountryID, out var id) || !eanIdsToIds.TryGetValue(eanCountry.ContinentID, out var toId)) continue;

                var regionToRegion = new RegionToRegion()
                {
                    Id = id,
                    ToId = toId,
                    CreatorId = creatorId
                };

                regionsToregions.Enqueue(regionToRegion);
            }

            return regionsToregions.ToArray();
        }

        private static LocalizedRegion[] BuildLocalizedRegions(
            IEnumerable<Country> eanCountries,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            int languageId,
            int creatorId)
        {

            var localizedRegions = new Queue<LocalizedRegion>();
            foreach (var eanCountry in eanCountries)
            {

                if (!eanIdsToIds.TryGetValue(eanCountry.CountryID, out var id)) continue;

                var localizedRegion = new LocalizedRegion
                {
                    Id = id,
                    LanguageId = languageId,
                    Name = eanCountry.CountryName,
                    CreatorId = creatorId
                };

                localizedRegions.Enqueue(localizedRegion);

            }


            return localizedRegions.ToArray();
        }

        private static Region[] BuildRegions(
            IEnumerable<Country> eanCountries,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            int creatorId
            )
        {

            var regions = new Queue<Region>();
            foreach (var eanCountry in eanCountries)
            {
                if (eanIdsToIds.ContainsKey(eanCountry.CountryID)) continue;

                var region = new Region
                {
                    EanId = eanCountry.CountryID,
                    CreatorId = creatorId
                };

                regions.Enqueue(region);
            }

            return regions.ToArray();

        }

       

        public Travel.DTO.Country[] BuildCountries(Country[] eanCountries,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            int creatorId)
        {
            var countries = new Queue<Travel.DTO.Country>();

            foreach (var eanCountry in eanCountries)
            {
                if (!eanIdsToIds.TryGetValue(eanCountry.CountryID, out var id)) continue;
                var country = new Travel.DTO.Country
                {
                    Id = id,
                    Code = eanCountry.CountryCode,
                    CreatorId = creatorId
                };
                countries.Enqueue(country);
            }

            return countries.ToArray();
        }
    }
}