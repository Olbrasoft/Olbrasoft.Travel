using System;
using System.Collections.Generic;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class LocalizedTypesOfRoomsImporter : Importer
    {

        private IReadOnlyDictionary<int, int> _accommodationsEanIdsToIds;

        private IReadOnlyDictionary<int, int> AccommodationsEanIdsToIds
        {
            get =>

                _accommodationsEanIdsToIds ?? (_accommodationsEanIdsToIds =
                    FactoryOfRepositories.MappedEntities<Accommodation>().EanIdsToIds);

            set => _accommodationsEanIdsToIds = value;
        }

        private IReadOnlyDictionary<int, int> _eanRoomTypeIdsToIds;

        protected IReadOnlyDictionary<int, int> EanRoomTypeIdsToIds
        {
            get => _eanRoomTypeIdsToIds ??
               (_eanRoomTypeIdsToIds = FactoryOfRepositories.MappedEntities<TypeOfRoom>().EanIdsToIds);

            set => _eanRoomTypeIdsToIds = value;
        }

        protected Queue<LocalizedTypeOfRoom> LocalizedTypesOfRooms = new Queue<LocalizedTypeOfRoom>();

        public LocalizedTypesOfRoomsImporter(IProvider provider, IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger)
            : base(provider, factoryOfRepositories, sharedProperties, logger)
        {
        }

        protected override void RowLoaded(string[] items)
        {
            if (!int.TryParse(items[0], out var eanHotelId) || !AccommodationsEanIdsToIds.ContainsKey(eanHotelId)) return;

            if (!int.TryParse(items[1], out var eanRoomTypeId) || !EanRoomTypeIdsToIds.TryGetValue(eanRoomTypeId, out var id)) return;

            var localizedTypeOfRoom = new LocalizedTypeOfRoom
            {
                Id = id,
                LanguageId = DefaultLanguageId,
                Name = items[4].Trim(),
                Description = items[5].Trim(),
                CreatorId = CreatorId
            };

            LocalizedTypesOfRooms.Enqueue(localizedTypeOfRoom);
        }

        public override void Import(string path)
        {
            LoadData(path);

            LogSave<LocalizedTypeOfRoom>();
            FactoryOfRepositories.Localized<LocalizedTypeOfRoom>().BulkSave(LocalizedTypesOfRooms, 270000);
            LogSaved<LocalizedTypeOfRoom>();
            
        }


        public override void Dispose()
        {
            EanRoomTypeIdsToIds = null;
            AccommodationsEanIdsToIds = null;
            LocalizedTypesOfRooms = null;

            GC.SuppressFinalize(this);
            base.Dispose();
        }

    }
}
