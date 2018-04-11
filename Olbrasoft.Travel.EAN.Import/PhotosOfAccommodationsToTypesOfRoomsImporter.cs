using System;
using System.Collections.Generic;
using System.IO;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class PhotosOfAccommodationsToTypesOfRoomsImporter:Importer
    {
        private IReadOnlyDictionary<int, int> _typesOfRoomsEanIdsToIds;

        protected IReadOnlyDictionary<int, int> TypesOfRoomsEanIdsToIds
        {
            get =>
                _typesOfRoomsEanIdsToIds ??
                (_typesOfRoomsEanIdsToIds = FactoryOfRepositories.MappedEntities<TypeOfRoom>().EanIdsToIds);

            set => _typesOfRoomsEanIdsToIds = value;
        }

        
        private IReadOnlyDictionary<Tuple<int, string, int>, int> _pathIdsAndFileIdsAndExtensionIdsToIds;

        protected IReadOnlyDictionary<Tuple<int, string, int>, int> PathIdsAndFileIdsAndExtensionIdsToIds
        {
            get => _pathIdsAndFileIdsAndExtensionIdsToIds ?? (_pathIdsAndFileIdsAndExtensionIdsToIds =
                       FactoryOfRepositories.PhotosOfAccommodations().GetPathIdsAndFileIdsAndExtensionIdsToIds());
            
            set => _pathIdsAndFileIdsAndExtensionIdsToIds = value;
        }

        private IReadOnlyDictionary<string, int> _pathsToIds;

        public IReadOnlyDictionary<string, int> PathsToIds
        {
            get => _pathsToIds ?? (_pathsToIds = FactoryOfRepositories.PathsToPhotos().PathsToIds );

            private set => _pathsToIds = value;
        }


        private IReadOnlyDictionary<string, int> _extensionsToIds;
       

        public IReadOnlyDictionary<string, int> ExtensionsToIds
        {
            get => _extensionsToIds ?? (_extensionsToIds = FactoryOfRepositories.FilesExtensions().ExtensionsToIds);

            private set => _extensionsToIds = value;
        }


        protected Queue<PhotoOfAccommodationToTypeOfRoom> PhotosOfAccommodationsToTypesOfRooms = new Queue<PhotoOfAccommodationToTypeOfRoom>();

        public PhotosOfAccommodationsToTypesOfRoomsImporter(IProvider provider, IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger) 
            : base(provider, factoryOfRepositories, sharedProperties, logger)
        {
        }

        protected override void RowLoaded(string[] items)
        {
            var url = items[3].Trim();

            if (string.IsNullOrEmpty(url) ||
                !PathsToIds.TryGetValue(ParsePath(url), out var pathToPhotoId) ||
                !ExtensionsToIds.TryGetValue(Path.GetExtension(url).ToLower(),
                    out var fileExtensionId)) return;

            var tup = new Tuple<int, string, int>(pathToPhotoId, RebuildFileName(url), fileExtensionId);
            if (!PathIdsAndFileIdsAndExtensionIdsToIds.TryGetValue(tup, out var photoOfAccommodationId)) return;

            if (!int.TryParse(items[1], out var eanRoomTypeId) ||
                !TypesOfRoomsEanIdsToIds.TryGetValue(eanRoomTypeId, out var typeOfRoomId)) return;

            var photoOfAccommodationToTypeOfRoom= new PhotoOfAccommodationToTypeOfRoom
            {
                Id = photoOfAccommodationId,
                ToId = typeOfRoomId,
                CreatorId = CreatorId
            };
            
            PhotosOfAccommodationsToTypesOfRooms.Enqueue(photoOfAccommodationToTypeOfRoom);
            
        }

        public override void Import(string path)
        {
            LoadData(path);

            var count = PhotosOfAccommodationsToTypesOfRooms.Count;

            WriteLog($"Builded {count} {typeof(PhotoOfAccommodation)}.");

            if (count <= 0) return;
            LogSave<PhotoOfAccommodationToTypeOfRoom>();
            FactoryOfRepositories.ManyToMany<PhotoOfAccommodationToTypeOfRoom>().BulkSave(PhotosOfAccommodationsToTypesOfRooms);
            LogSaved<PhotoOfAccommodationToTypeOfRoom>();

        }

        public override void Dispose()
        {
            PhotosOfAccommodationsToTypesOfRooms = null;
            PathIdsAndFileIdsAndExtensionIdsToIds = null;
            PathsToIds = null;
            ExtensionsToIds = null;
            TypesOfRoomsEanIdsToIds = null;

            GC.SuppressFinalize(this);
            base.Dispose();
        }

    }
}