using System;
using System.Collections.Generic;
using Olbrasoft.Travel.Data.Entity;
using Olbrasoft.Travel.DataAccessLayer;
using Attribute = Olbrasoft.Travel.Data.Entity.Attribute;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.Import
{
    public class AccommodationsToAttributesDefaultLanguageImporter : Importer
    {
        private IReadOnlyDictionary<int, int> _accommodationsEanIdsToIds;
        private IReadOnlyDictionary<int, int> _attributesEanIdsToIds;

        protected IReadOnlyDictionary<int, int> AttributesEanIdsToIds
        {
            get => _attributesEanIdsToIds ?? (_attributesEanIdsToIds =
                       FactoryOfRepositories.MappedEntities<Attribute>().EanIdsToIds);

            set => _attributesEanIdsToIds = value;
        }

        protected IReadOnlyDictionary<int, int> AccommodationsEanIdsToIds
        {
            get => _accommodationsEanIdsToIds ?? (_accommodationsEanIdsToIds =
                       FactoryOfRepositories.MappedEntities<Accommodation>().EanIdsToIds);

            set => _accommodationsEanIdsToIds = value;
        }

        protected Queue<AccommodationToAttribute> AccommodationsToAttributes = new Queue<AccommodationToAttribute>();

        public AccommodationsToAttributesDefaultLanguageImporter(IProvider provider, IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger)
            : base(provider, factoryOfRepositories, sharedProperties, logger)
        {
        }

        protected override void RowLoaded(string[] items)
        {
            if (!int.TryParse(items[0], out var eanHotelId) ||
                !AccommodationsEanIdsToIds.TryGetValue(eanHotelId, out var accommodationId) ||
                !int.TryParse(items[1], out var eanAttributeId) ||
                !AttributesEanIdsToIds.TryGetValue(eanAttributeId, out var attributeId)
            ) return;

            var accommodationToAttribute = new AccommodationToAttribute
            {
                AccommodationId = accommodationId,
                AttributeId = attributeId,
                LanguageId = DefaultLanguageId,
                Text = items[3],
                CreatorId = CreatorId
            };

            AccommodationsToAttributes.Enqueue(accommodationToAttribute);
        }

        public override void Import(string path)
        {
            LoadData(path);
            AttributesEanIdsToIds = null;
            AccommodationsEanIdsToIds = null;

            if (AccommodationsToAttributes.Count <= 0) return;

            LogSave<AccommodationToAttribute>();
            FactoryOfRepositories.AccommodationsToAttributes().BulkSave(AccommodationsToAttributes);
            AccommodationsToAttributes = null;
            LogSaved<AccommodationToAttribute>();
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            base.Dispose();
        }

    }
}