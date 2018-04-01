using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Olbrasoft.Travel.DAL.EntityFramework;
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



            var pathsToPhotosRepository = FactoryOfRepositories.PathsToPhotos();
            LogBuild<PathToPhoto>();
            var pathsToPhotos = BuildPathsToPhotos(roomsTypes, pathsToPhotosRepository.Paths, CreatorId);
            var count = pathsToPhotos.Length;
            LogBuilded(count);

            if (count > 0)
            {
                LogSave<PathToPhoto>();
                pathsToPhotosRepository.BulkSave(pathsToPhotos);
                LogSaved<PathToPhoto>();
            }



            //LogBuild<TypeOfRoom>();
            //var typesOfRooms = BuildTypesOfRooms(roomsTypes, CreatorId);
            //var count = typesOfRooms.Length;
            //LogBuilded(count);

            //LogSave<TypeOfRoom>();
            //using (var ctx = new TravelContext())
            //{
            //   ctx.BulkInsert(typesOfRooms,OnSaved);
            //}
            //LogSaved<TypeOfRoom>();

        }


        private static PathToPhoto[] BuildPathsToPhotos(IEnumerable<RoomType> eanEntities,
            ICollection<string> paths,
            int creatorId
        )
        {
            var group = eanEntities.Where(p=>!string.IsNullOrEmpty(p.RoomTypeImage)).Select(p => ParsePath(p.RoomTypeImage)).Distinct();

            return group.Where(ptp => !paths.Contains(ptp)).Select(ptp => new PathToPhoto()
            {
                Path = ptp,
                CreatorId = creatorId

            }).ToArray();
        }


        TypeOfRoom[] BuildTypesOfRooms(IEnumerable<RoomType> roomTypes,

            int creatorId
            )
        {
            var typesOfRooms = new Queue<TypeOfRoom>();

            foreach (var roomType in roomTypes)
            {
                var typeOfRoom = new TypeOfRoom()
                {
                    EANHotelID = roomType.EANHotelID,
                    RoomTypeID = roomType.RoomTypeID,
                    LanguageCode = roomType.LanguageCode,
                    RoomTypeImage = roomType.RoomTypeImage,
                    RoomTypeName = roomType.RoomTypeName,
                    RoomTypeDescription = roomType.RoomTypeDescription,
                    CreatorId = creatorId

                };

                typesOfRooms.Enqueue(typeOfRoom);

            }

            return typesOfRooms.ToArray();
        }


    }
}
