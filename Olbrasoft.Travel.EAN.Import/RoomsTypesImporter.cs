using System;
using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.DAL;
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
            var eanHotelIdsToIds = FactoryOfRepositories.MappedEntities<Accommodation>().EanIdsToIds;
            var eanHotelIds = new HashSet<int>(eanHotelIdsToIds.Keys);

            roomsTypes = roomsTypes.Where(rt => eanHotelIds.Contains(rt.EANHotelID)).ToArray();

            var eanRoomTypeIdsToIds = ImportTypesOfRooms(roomsTypes, FactoryOfRepositories.MappedEntities<TypeOfRoom>(),
                eanHotelIdsToIds, CreatorId);
            


            //var urls = roomsTypes.Where(p => !string.IsNullOrEmpty(p.RoomTypeImage)).Select(p => p.RoomTypeImage).ToArray();

            //var pathsToIds = ImportPathsToPhotos(urls, FactoryOfRepositories.PathsToPhotos(), CreatorId);

            //var extensionsToIds = ImportFilesExtensions(urls, FactoryOfRepositories.FilesExtensions(), CreatorId);

            //ImportPhotosOfAccommodations(roomsTypes.Where(rt => !string.IsNullOrEmpty(rt.RoomTypeImage)),
            //    FactoryOfRepositories.PhotosOfAccommodations(), eanRoomTypeIdsToIds, eanHotelIdsToIds, pathsToIds,
            //    extensionsToIds, CreatorId);
        }

        

        private void ImportPhotosOfAccommodations(IEnumerable<RoomType> roomsTypes,
            IPhotosOfAccommodationsRepository repository,
            IReadOnlyDictionary<int, int> eanRoomTypeIdsToIds,
            IReadOnlyDictionary<int, int> eanHotelIdsToIds,
            IReadOnlyDictionary<string, int> pathsToIds,
            IReadOnlyDictionary<string, int> extensionsToIds,
            int creatorId
        )
        {
            LogBuild<PhotoOfAccommodation>();
            var photosOfAccommodations = BuildPhotosOfAccommodations(
                roomsTypes.Where(rt => !string.IsNullOrEmpty(rt.RoomTypeImage)), eanRoomTypeIdsToIds, eanHotelIdsToIds,
                pathsToIds, extensionsToIds, creatorId);
            var count = photosOfAccommodations.Length;
            LogBuilded(count);

            if (count <= 0) return;

            LogSave<PhotoOfAccommodation>();
            repository.BulkSave(photosOfAccommodations,poa=>poa.CaptionId,poa=>poa.IsDefault);
            LogSaved<PhotoOfAccommodation>();

        }


        private IReadOnlyDictionary<int, int> ImportTypesOfRooms(IEnumerable<RoomType> roomsTypes,
            IMappedEntitiesRepository<TypeOfRoom> repository,
            IReadOnlyDictionary<int, int> eanHotelsIdsToIds,
            int creatorId
        )
        {
            LogBuild<TypeOfRoom>();
            var typesOfRooms = BuildTypesOfRooms(roomsTypes, eanHotelsIdsToIds, creatorId);
            var count = typesOfRooms.Length;
            LogBuilded(count);

            if (count <= 0) return repository.EanIdsToIds;

            LogSave<TypeOfRoom>();
            repository.BulkSave(typesOfRooms);
            LogSaved<TypeOfRoom>();

            return repository.EanIdsToIds;
        }


        private static PhotoOfAccommodation[] BuildPhotosOfAccommodations(IEnumerable<RoomType> roomsTypes,
            IReadOnlyDictionary<int, int> eanRoomTypeIdsToIds,
            IReadOnlyDictionary<int, int> eanHotelIdsToIds,
            IReadOnlyDictionary<string, int> pathsToIds,
            IReadOnlyDictionary<string, int> extensionsToIds,
            int creatorId
        )
        {
            var photosOfAccommodations = new Queue<PhotoOfAccommodation>();

            foreach (var roomType in roomsTypes)
            {
                if( string.IsNullOrEmpty(roomType.RoomTypeImage) ||    
                   !eanRoomTypeIdsToIds.TryGetValue(roomType.RoomTypeID,out var typeOfRoomId) ||
                   !eanHotelIdsToIds.TryGetValue(roomType.EANHotelID,out var accommodationId) ||
                   !pathsToIds.TryGetValue(ParsePath(roomType.RoomTypeImage),out var pathId) ||
                   !extensionsToIds.TryGetValue(System.IO.Path.GetExtension(roomType.RoomTypeImage).ToLower(),out var extensionId) 
                   ) continue;

                var photoOfAccommodation = new PhotoOfAccommodation
                {
                    AccommodationId = accommodationId,
                    TypeOfRoomId = typeOfRoomId,
                    PathToPhotoId = pathId,
                    FileName = System.IO.Path.GetFileNameWithoutExtension(roomType.RoomTypeImage),
                    FileExtensionId = extensionId,
                    CreatorId = creatorId
                };

                photosOfAccommodations.Enqueue(photoOfAccommodation);
            }

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
