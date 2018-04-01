using System;
using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Property;

namespace Olbrasoft.Travel.EAN.Import
{
    class RoomsTypesImporter : BatchImporter<RoomType>
    {
        public RoomsTypesImporter(ImportOption option) : base(option)
        {
            BatchSize = 320000;
        }

        protected void OnSaved(EventArgs eventArgs)
        {
        }

        public override void ImportBatch(RoomType[] roomsTypes)
        {

            LogBuild<TypeOfRoom>();
            var typesOfRooms = BuildTypesOfRooms(roomsTypes, FactoryOfRepositories.MappedEntities<Accommodation>().EanIdsToIds, CreatorId);
            var count = typesOfRooms.Length;
            LogBuilded(count);

            var typesOfRoomsRepository = FactoryOfRepositories.MappedEntities<TypeOfRoom>();

            if (count > 0)
            {
                LogSave<TypeOfRoom>();
                typesOfRoomsRepository.BulkSave(typesOfRooms);
                LogSaved<TypeOfRoom>();
            }


            var urls = roomsTypes.Where(p => !string.IsNullOrEmpty(p.RoomTypeImage)).Select(p => p.RoomTypeImage).ToArray();
            
            var pathsToIds = ImportPathsToPhotos(urls, FactoryOfRepositories.PathsToPhotos(), CreatorId);

            var extensionsToIds = ImportFilesExtensions(urls, FactoryOfRepositories.FilesExtensions(), CreatorId);




        }

        
       



        private static PhotoOfAccommodation[] BuildPhotosOfAccommodations(IEnumerable<RoomType> roomsTypes,
        IReadOnlyDictionary<string, int> pathsToIds,
        IReadOnlyDictionary<string, int> extensionsToIds,
        int creatorId
        )
        {
            var photosOfAccommodations = new Queue<PhotoOfAccommodation>();

            return photosOfAccommodations.ToArray();
        }



        private static TypeOfRoom[] BuildTypesOfRooms(IEnumerable<RoomType> roomTypes,
            IReadOnlyDictionary<int, int> accommodationsEnaIdsToIds,
                int creatorId
            )
        {
            var typesOfRooms = new Queue<TypeOfRoom>();

            foreach (var roomType in roomTypes)
            {
                if (!accommodationsEnaIdsToIds.TryGetValue(roomType.EANHotelID, out var accommodationId)) continue;

                var typeOfRoom = new TypeOfRoom()
                {
                    EanId = roomType.RoomTypeID,
                    AccommodationId = accommodationId,
                    CreatorId = creatorId

                };

                typesOfRooms.Enqueue(typeOfRoom);

            }

            return typesOfRooms.ToArray();
        }


    }
}
