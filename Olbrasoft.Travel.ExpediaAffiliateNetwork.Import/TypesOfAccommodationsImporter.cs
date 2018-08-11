﻿using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.Data.Entity;
using Olbrasoft.Travel.DataAccessLayer;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.Import
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

            var typesOfAccommodationsEanIdsToIds = ImportTypesOfAccommodations(EanIdsToNames.Keys,
                FactoryOfRepositories.MappedEntities<TypeOfAccommodation>(), CreatorId);

            ImportLocalizedTypesOfAccommodations(EanIdsToNames,
                FactoryOfRepositories.Localized<LocalizedTypeOfAccommodation>(), typesOfAccommodationsEanIdsToIds, DefaultLanguageId,
                CreatorId);

            EanIdsToNames = null;
        }


        private void ImportLocalizedTypesOfAccommodations(IDictionary<int, string> eanIdsToNames,
            ILocalizedRepository<LocalizedTypeOfAccommodation> repository,
            IReadOnlyDictionary<int, int> typesOfAccommodationsEanIdsToIds,
            int languageId,
            int creatorId)
        {
            LogBuild<LocalizedTypeOfAccommodation>();
            var localizedTypesOfAccommodations = BuildLocalizedTypesOfAccommodations(eanIdsToNames,
                typesOfAccommodationsEanIdsToIds, languageId, creatorId);
            var count = localizedTypesOfAccommodations.Length;

            if (count <= 0) return;

            LogSave<LocalizedTypeOfAccommodation>();
            repository.BulkSave(localizedTypesOfAccommodations, count);
            LogSaved<LocalizedTypeOfAccommodation>();
        }


        private static LocalizedTypeOfAccommodation[] BuildLocalizedTypesOfAccommodations(
            IDictionary<int, string> eanIdsToNames,
            IReadOnlyDictionary<int, int> typesOfAccommodationsEanIdsToIds,
            int languageId,
            int creatorId
        )
        {
            var localizedTypesOfAccommodations = new Queue<LocalizedTypeOfAccommodation>();

            foreach (var propertyType in eanIdsToNames)
            {
                if (!typesOfAccommodationsEanIdsToIds.TryGetValue(propertyType.Key, out var id)) continue;

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
            var typesOfAccommodations = BuildTypesOfAccommodations(eanIds, creatorId);
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