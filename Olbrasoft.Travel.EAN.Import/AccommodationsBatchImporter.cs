using System.Collections.Generic;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Property;

namespace Olbrasoft.Travel.EAN.Import
{
    /// <summary>
    /// todo https://www.currency-iso.org/dam/downloads/lists/list_one.xml
    /// </summary>
    class AccommodationsBatchImporter :BatchImporter<Travel.EAN.DTO.Property.ActiveProperty>
    {
        public AccommodationsBatchImporter(ImportOption option) : base(option)
        {
        }

        public override void ImportBatch(ActiveProperty[] eanEntities)
        {

            var typeOfAccommodationEanIdsToIds =
                FactoryOfRepositories.MappedEntities<TypeOfAccommodation>().EanIdsToIds;

            var chainsEanIdsToIds = FactoryOfRepositories.MappedEntities<Travel.DTO.Chain>().EanIdsToIds;

            var countriesCodesToIds = FactoryOfRepositories.Countries().CodesToIds;
            

            //todo Jedno ubytovani pravdepodobne neni validni vraci to builded 79 999 misto 80 000 
            LogBuild<Accommodation>();
            var accommodations = BuildAccommodations(eanEntities,typeOfAccommodationEanIdsToIds,chainsEanIdsToIds, countriesCodesToIds,
                FactoryOfRepositories.BaseNames<Travel.DTO.Chain>().GetId("Independent"),
                CreatorId);
            var count = accommodations.Length;
            LogBuilded(count);

            var accommodationsRepository = FactoryOfRepositories.MappedEntities<Accommodation>();
            
            if (count > 0)
            {
                LogSave<Accommodation>();
                accommodationsRepository.BulkSave(accommodations);
                LogSaved<Accommodation>();
            }

            LogBuild<LocalizedAccommodation>();
            var localizedAccommodations = BuildLocalizedAccommodations(eanEntities,
                accommodationsRepository.EanIdsToIds, DefaultLanguageId, CreatorId);
            count = localizedAccommodations.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSave<LocalizedAccommodation>();
            FactoryOfRepositories.Localized<LocalizedAccommodation>().BulkSave(localizedAccommodations);
            LogSaved<LocalizedAccommodation>();
            
        }


        LocalizedAccommodation[] BuildLocalizedAccommodations(IEnumerable<ActiveProperty> activeProperties,
            IReadOnlyDictionary<int,int> eanIdsToIds,
            int languageId,
            int creatorId
            )
        {
            var localizedAccommodations = new Queue<LocalizedAccommodation>();

            foreach (var activeProperty in activeProperties)
            {
                
                if(!eanIdsToIds.TryGetValue(activeProperty.EANHotelID,out var id)) continue;

                var localizedAccommodation = new LocalizedAccommodation()
                {
                    Id = id,
                    LanguageId = languageId,
                    Name = activeProperty.Name,
                    Location = activeProperty.Location,
                    CheckInTime = activeProperty.CheckInTime,
                    CheckOutTime = activeProperty.CheckOutTime,
                    CreatorId = creatorId
                };

                localizedAccommodations.Enqueue(localizedAccommodation);
                
            }

            return localizedAccommodations.ToArray();

        }


        Accommodation[] BuildAccommodations(IEnumerable<ActiveProperty> activeProperties,
        IReadOnlyDictionary<int,int> typesOfAccommodationsEanIdsToIds,
        IReadOnlyDictionary<int,int> chainsEanIdsToIds,
        IReadOnlyDictionary<string,int> countriesCodesToIds,
        int chainIndependentId,
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
                        SequenceNumber = activeProperty.SequenceNumber,
                        EanId = activeProperty.EANHotelID,
                        Address = activeProperty.Address1,
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
                    else
                    {
                        accommodation.ChainId = chainIndependentId;
                    }

                    accommodations.Enqueue(accommodation);
                }
                else
                {
                   Logger.Log(activeProperty.EANHotelID.ToString() + "Neproslo type" + activeProperty.PropertyCategory + " countr " + activeProperty.Country );
                }
                
            }

            return accommodations.ToArray();
        }


    }
}
