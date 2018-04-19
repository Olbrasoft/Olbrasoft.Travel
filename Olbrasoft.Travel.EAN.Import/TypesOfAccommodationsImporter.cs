using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class TypesOfAccommodationsImporter : Importer
    {

        protected IDictionary<int, string> EanIdsToNames = new Dictionary<int, string>();

        public TypesOfAccommodationsImporter(IProvider provider, IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger)
            : base(provider, factoryOfRepositories, sharedProperties, logger)
        {
        }

        protected override void RowLoaded(string[] items)
        {
            if (!int.TryParse(items[0], out var eanId)) return;

            EanIdsToNames.Add(eanId, items[2]);
        }

        public override void Import(string path)
        {
            LoadData(path);

            var eanIdsToIds = ImportTypesOfAccommodations(EanIdsToNames.Keys,
                FactoryOfRepositories.MappedEntities<TypeOfAccommodation>(), CreatorId);

            ImportLocalizedTypesOfAccommodations(EanIdsToNames,
                FactoryOfRepositories.Localized<LocalizedTypeOfAccommodation>(), eanIdsToIds, DefaultLanguageId,
                CreatorId);

            EanIdsToNames = null;
        }


        private void ImportLocalizedTypesOfAccommodations(IDictionary<int, string> eanIdsToNames,
            ILocalizedRepository<LocalizedTypeOfAccommodation> repository,
            IReadOnlyDictionary<int, int> eanIdsToIds,
            int languageId,
            int creatorId)
        {
            LogBuild<LocalizedTypeOfAccommodation>();
            var localizedTypesOfAccommodations = BuildLocalizedTypesOfAccommodations(eanIdsToNames,
                eanIdsToIds, languageId, creatorId);
            var count = localizedTypesOfAccommodations.Length;

            if (count <= 0) return;

            LogSave<LocalizedTypeOfAccommodation>();
            repository.BulkSave(localizedTypesOfAccommodations, count);
            LogSaved<LocalizedTypeOfAccommodation>();
        }


        private static LocalizedTypeOfAccommodation[] BuildLocalizedTypesOfAccommodations(
            IDictionary<int, string> eanIdsToNames,
            IReadOnlyDictionary<int, int> eanIdsToIds,
            int languageId,
            int creatorId
        )
        {
            var localizedTypesOfAccommodations = new Queue<LocalizedTypeOfAccommodation>();

            foreach (var propertyType in eanIdsToNames)
            {
                if (!eanIdsToIds.TryGetValue(propertyType.Key, out var id)) continue;

                var localizedTypeOfAccommodation = new LocalizedTypeOfAccommodation
                {
                    Id = id,
                    LanguageId = languageId,
                    Name = propertyType.Value,
                    CreatorId = creatorId
                };

                localizedTypesOfAccommodations.Enqueue(localizedTypeOfAccommodation);

            }
            return localizedTypesOfAccommodations.ToArray();
        }

        private IReadOnlyDictionary<int, int> ImportTypesOfAccommodations(
            IEnumerable<int> eanIds,
            IMappedEntitiesRepository<TypeOfAccommodation> repository,
            int creatorId
        )
        {
            LogBuild<TypeOfAccommodation>();
            var typesOfAccommodations = BuildTypesOfAccommodations(eanIds, CreatorId);
            var count = typesOfAccommodations.Length;
            LogBuilded(count);


            if (count <= 0) return repository.EanIdsToIds;

            LogSave<TypeOfAccommodation>();
            repository.BulkSave(typesOfAccommodations);
            LogSaved<TypeOfAccommodation>();

            return repository.EanIdsToIds;
        }


        private static TypeOfAccommodation[] BuildTypesOfAccommodations(IEnumerable<int> eanIds,
            int creatorId
        )
        {
            return eanIds.Select(ei => new TypeOfAccommodation
            {
                EanId = ei,
                CreatorId = creatorId
            }).ToArray();
        }

    }
}