﻿using System.Collections.Generic;
using Olbrasoft.Travel.Data.Entity;
using Olbrasoft.Travel.DataAccessLayer;
using Olbrasoft.Travel.ExpediaAffiliateNetwork.DataTransferObject.Property;
using Chain = Olbrasoft.Travel.Data.Entity.Chain;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.Import
{
    /// <summary>
    /// todo https://www.currency-iso.org/dam/downloads/lists/list_one.xml
    /// </summary>
    internal class AccommodationsImporter : Importer<ActiveProperty>
    {
        private IReadOnlyDictionary<int, int> _typesOfAccommodationsEanIdsToIds;
        private IReadOnlyDictionary<int, int> _chainsEanIdsToIds;
        private IReadOnlyDictionary<string, int> _countriesCodesToIds;
        private IReadOnlyDictionary<string, int> _airportsCodesToIds;

        protected IReadOnlyDictionary<int, int> TypesOfAccommodationsEanIdsToIds
        {
            get => _typesOfAccommodationsEanIdsToIds ?? (_typesOfAccommodationsEanIdsToIds =
                       FactoryOfRepositories.MappedEntities<TypeOfAccommodation>().EanIdsToIds);

            set => _typesOfAccommodationsEanIdsToIds = value;
        }

        protected IReadOnlyDictionary<int, int> ChainsEanIdsToIds
        {
            get => _chainsEanIdsToIds ??
                   (_chainsEanIdsToIds = FactoryOfRepositories.MappedEntities<Chain>().EanIdsToIds);

            set => _chainsEanIdsToIds = value;
        }


        protected IReadOnlyDictionary<string, int> CountriesCodesToIds
        {
            get => _countriesCodesToIds ??
                   (_countriesCodesToIds = FactoryOfRepositories.AdditionalRegionsInfo<Country>().CodesToIds);

            set => _countriesCodesToIds = value;
        }


        protected IReadOnlyDictionary<string, int> AirportsCodesToIds
        {
            get => _airportsCodesToIds ??
                   (_airportsCodesToIds = FactoryOfRepositories.AdditionalRegionsInfo<Airport>().CodesToIds);

            set => _airportsCodesToIds = value;
        }
        
        public AccommodationsImporter(IProvider provider, IParserFactory parserFactory,
            IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger)
            : base(provider, parserFactory, factoryOfRepositories, sharedProperties, logger)
        {
        }

        public override void Import(string path)
        {
            LoadData(path);

            var accommodationsEanIdsToIds = ImportAccommodations(EanDataTransferObjects,
                FactoryOfRepositories.MappedEntities<Accommodation>(), TypesOfAccommodationsEanIdsToIds,
                CountriesCodesToIds, AirportsCodesToIds, ChainsEanIdsToIds, CreatorId);

            TypesOfAccommodationsEanIdsToIds = null;
            CountriesCodesToIds = null;
            AirportsCodesToIds = null;
            ChainsEanIdsToIds = null;

            ImportLocalizedAccommodations(EanDataTransferObjects, FactoryOfRepositories.Localized<LocalizedAccommodation>(),
                accommodationsEanIdsToIds, DefaultLanguageId, CreatorId);

            EanDataTransferObjects = null;
        }
        
        private IReadOnlyDictionary<int, int> ImportAccommodations(
            IEnumerable<ActiveProperty> activeProperties,
            IMappedEntitiesRepository<Accommodation> repository,
            IReadOnlyDictionary<int, int> typesOfAccommodationsEanIdsToIds,
            IReadOnlyDictionary<string, int> countriesCodesToIds,
            IReadOnlyDictionary<string, int> airportsCodesToIds,
            IReadOnlyDictionary<int, int> chainsEanIdsToIds,
            int creatorId
        )
        {
            //todo Jedno ubytovani pravdepodobne neni validni vraci to builded 79 999 misto 80 000 
            LogBuild<Accommodation>();
            var accommodations = BuildAccommodations(
                activeProperties,
                typesOfAccommodationsEanIdsToIds,
                countriesCodesToIds,
                airportsCodesToIds,
                chainsEanIdsToIds,
                creatorId
            );
            var count = accommodations.Length;
            LogBuilded(count);


            if (count <= 0) return repository.EanIdsToIds;

            LogSave<Accommodation>();
            repository.BulkSave(accommodations);
            LogSaved<Accommodation>();

            return repository.EanIdsToIds;
        }

        private Accommodation[] BuildAccommodations(
            IEnumerable<ActiveProperty> activeProperties,
            IReadOnlyDictionary<int, int> typesOfAccommodationsEanIdsToIds,
            IReadOnlyDictionary<string, int> countriesCodesToIds,
            IReadOnlyDictionary<string, int> airportsCodesToIds,
            IReadOnlyDictionary<int, int> chainsEanIdsToIds,
            int creatorId
        )
        {
            var accommodations = new Queue<Accommodation>();

            foreach (var activeProperty in activeProperties)
            {
                if (typesOfAccommodationsEanIdsToIds.TryGetValue(activeProperty.PropertyCategory,
                        out var typeOfAccommodationId) &&
                    countriesCodesToIds.TryGetValue(activeProperty.Country, out var countryId))
                {
                    var accommodation = new Accommodation
                    {
                        StarRating = activeProperty.StarRating,
                        SequenceNumber = activeProperty.SequenceNumber,
                        EanId = activeProperty.EANHotelID,
                        Address = activeProperty.Address1,
                        AdditionalAddress = activeProperty.Address2,
                        CountryId = countryId,
                        TypeOfAccommodationId = typeOfAccommodationId,
                        CenterCoordinates = CreatePoint(activeProperty.Latitude, activeProperty.Longitude),
                        CreatorId = creatorId
                    };

                    if (!string.IsNullOrEmpty(activeProperty.ChainCodeID) &&
                        chainsEanIdsToIds.TryGetValue(int.Parse(activeProperty.ChainCodeID), out var chainId))
                    {
                        accommodation.ChainId = chainId;
                    }

                    if (!string.IsNullOrEmpty(activeProperty.AirportCode) &&
                        airportsCodesToIds.TryGetValue(activeProperty.AirportCode, out var airportId))
                    {
                        accommodation.AirportId = airportId;
                    }

                    if (string.IsNullOrEmpty(accommodation.Address) &&
                        !string.IsNullOrEmpty(accommodation.AdditionalAddress))
                    {
                        accommodation.Address = accommodation.AdditionalAddress;
                        accommodation.AdditionalAddress = null;
                    }

                    if (string.IsNullOrEmpty(accommodation.Address))
                    {
                        accommodation.Address = "not entered";
                    }

                    accommodations.Enqueue(accommodation);
                }
                else
                {
                    Logger.Log(activeProperty.EANHotelID.ToString() + "Neproslo type" +
                               activeProperty.PropertyCategory + " countr " + activeProperty.Country);
                }
            }

            return accommodations.ToArray();
        }
    }
}