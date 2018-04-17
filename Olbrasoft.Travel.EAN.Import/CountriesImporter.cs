using System.Collections.Generic;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;
using Country = Olbrasoft.Travel.EAN.DTO.Geography.Country;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class CountriesImporter : Importer
    {
        public class CandidateCountry
        {
            public string Name { get; set; }
            public string Code { get; set; }
        }

        private static IEnumerable<CandidateCountry> ProbablyMissingCoutries => new[]
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

        private readonly IParserFactory _parserFactory;
        protected IParser<Country> Parser;
        protected Queue<Country> EanCountries = new Queue<Country>();


        public CountriesImporter(IProvider provider, IParserFactory parserFactory, IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger)
            : base(provider, factoryOfRepositories, sharedProperties, logger)
        {
            _parserFactory = parserFactory;
        }

        protected override void RowLoaded(string[] items)
        {
            EanCountries.Enqueue(Parser.Parse(items));
        }

        public override void Import(string path)
        {
            Parser = _parserFactory.Create<Country>(Provider.GetFirstLine(path));

            LoadData(path);

            var eanCountries = EanCountries.ToArray();
            EanCountries = null;

            var regionsRepository = FactoryOfRepositories.Regions();
            var regionsEanIdsToIds = regionsRepository.EanIdsToIds;

            regionsEanIdsToIds = ImportRegions(eanCountries, regionsRepository, regionsEanIdsToIds);

            ImportLocalizedRegions(eanCountries, FactoryOfRepositories.Localized<LocalizedRegion>(), regionsEanIdsToIds);

            var typeOfRegionCountryId = FactoryOfRepositories.BaseNames<TypeOfRegion>().GetId("Country");

            ImportRegionsToTypes(eanCountries, FactoryOfRepositories.RegionsToTypes(), regionsEanIdsToIds, typeOfRegionCountryId);

            ImportRegionsToRegions(eanCountries, FactoryOfRepositories.ManyToMany<RegionToRegion>(), regionsEanIdsToIds);

            var countriesRepository = FactoryOfRepositories.AdditionalRegionsInfo<Travel.DTO.Country>();

            ImportCountries(eanCountries, countriesRepository, regionsEanIdsToIds);

            ImportProbablyMissingCoutries(ProbablyMissingCoutries, countriesRepository, typeOfRegionCountryId, regionsRepository);
        }


        private void ImportProbablyMissingCoutries(IEnumerable<CandidateCountry> probablyMissingCoutries,
            IAdditionalRegionsInfoRepository<Travel.DTO.Country> countriesRepository, int typeOfRegionCountryId,
            IRegionsRepository regionsRepository)
        {
            foreach (var probablyMissingCoutry in probablyMissingCoutries)
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

        private void ImportCountries(Country[] eanCountries,
            IAdditionalRegionsInfoRepository<Travel.DTO.Country> repository,
            IReadOnlyDictionary<long, int> regionsEanIdsToIds
           )
        {
            LogBuild<Travel.DTO.Country>();
            var countries = BuildCountries(eanCountries, regionsEanIdsToIds, CreatorId);
            var count = countries.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<Travel.DTO.Country>();
            repository.BulkSave(countries);
            LogSaved<Travel.DTO.Country>();
        }

        private void ImportRegionsToRegions(
            Country[] eanCountries,
            IManyToManyRepository<RegionToRegion> repository,
            IReadOnlyDictionary<long, int> regionsEanIdsToIds)
        {
            Logger.Log($"Build RegionsToRegions from CoutryList");
            var regionsToRegions = BuildRegionsToRegions(eanCountries, regionsEanIdsToIds, CreatorId);
            var count = regionsToRegions.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<RegionToRegion>();
            repository.BulkSave(regionsToRegions, count);
            LogSaved<RegionToRegion>();
        }

        private void ImportRegionsToTypes(
            Country[] eanCountries,
            IRegionsToTypesRepository repository,
            IReadOnlyDictionary<long, int> regionsEanIdsToIds,
            int typeOfRegionCountryId
            )
        {
            LogBuild<RegionToType>();
            var regionsToTypes = BuildRegionsToTypes(eanCountries, regionsEanIdsToIds, typeOfRegionCountryId, CreatorId);
            var count = regionsToTypes.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<RegionToType>();
            repository.BulkSave(regionsToTypes);
            LogSaved<RegionToType>();
        }

        private void ImportLocalizedRegions(Country[] eanCountries, ILocalizedRepository<LocalizedRegion> repository, IReadOnlyDictionary<long, int> regionsEanIdsToIds)
        {
            Logger.Log("Build localizedRegions from CountryList");
            var localizedRegions = BuildLocalizedRegions(eanCountries, regionsEanIdsToIds, DefaultLanguageId, CreatorId);
            var count = localizedRegions.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<LocalizedRegion>();
            repository.BulkSave(localizedRegions, count);
            LogSaved<LocalizedRegion>();
        }

        private IReadOnlyDictionary<long, int> ImportRegions(
            Country[] eanCountries,
            IRegionsRepository regionsRepository,
            IReadOnlyDictionary<long, int> eanIdsToIds
            )
        {
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

            return eanIdsToIds;
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
    }
}