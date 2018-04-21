using System.Collections.Generic;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;
using Description = Olbrasoft.Travel.EAN.DTO.Property.Description;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class DescriptionsImporter : Importer
    {
        private IReadOnlyDictionary<int, int> _accommodationsEanIdsToIds;

        protected IReadOnlyDictionary<int, int> AccommodationsEanIdsToIds
        {
            get => _accommodationsEanIdsToIds ?? (_accommodationsEanIdsToIds =
                       FactoryOfRepositories.MappedEntities<Accommodation>().EanIdsToIds);

            set => _accommodationsEanIdsToIds = value;
        }

        private IReadOnlyDictionary<string, int> _languagesEanLanguageCodesToIds;

        protected IReadOnlyDictionary<string, int> LanguagesEanLangueageCodesToIds
        {
            get => _languagesEanLanguageCodesToIds ?? (_languagesEanLanguageCodesToIds =
                       FactoryOfRepositories.Languages().EanLanguageCodesToIds);

            set => _languagesEanLanguageCodesToIds = value;
        }

        protected int TypeOfDescriptionId { get; set; }

        protected Queue<Travel.DTO.Description> Descriptions = new Queue<Travel.DTO.Description>();
        
        public DescriptionsImporter(IProvider provider, IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger) 
            : base(provider, factoryOfRepositories, sharedProperties, logger)
        {
        }

        protected override void RowLoaded(string[] items)
        {
            if (
                !int.TryParse(items[0], out var eanHotelId) ||
                !AccommodationsEanIdsToIds.TryGetValue(eanHotelId, out var accommodationId) ||
                !LanguagesEanLangueageCodesToIds.TryGetValue(items[1], out var languageId)
            ) return;
            
            var description = new Travel.DTO.Description
            {
                AccommodationId = accommodationId,
                TypeOfDescriptionId = TypeOfDescriptionId,
                LanguageId = languageId,
                Text = items[2],
                CreatorId = CreatorId

            };

            Descriptions.Enqueue(description);
        }

        public override void Import(string path)
        {
            const string general = "General";
            var typesOfDescriptionsRepository = FactoryOfRepositories.BaseNames<TypeOfDescription>();

            if (!typesOfDescriptionsRepository.NamesToIds.ContainsKey(general))
            {
                typesOfDescriptionsRepository.Add(new TypeOfDescription {Name = general, CreatorId = CreatorId});
            }

            TypeOfDescriptionId = typesOfDescriptionsRepository.GetId(general);

            LoadData(path);

            if (Descriptions.Count <= 0) return;

            LogSave<Description>();
            FactoryOfRepositories.Descriptions().BulkSave(Descriptions);
            LogSaved<Description>();

        }



    }
}