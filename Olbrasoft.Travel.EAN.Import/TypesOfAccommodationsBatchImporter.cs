using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Property;

namespace Olbrasoft.Travel.EAN.Import
{
    class TypesOfAccommodationsBatchImporter : BatchImporter<PropertyType>
    {




        public TypesOfAccommodationsBatchImporter(ImportOption option) : base(option)
        {
        }

        public override void ImportBatch(PropertyType[] eanEntities)
        {
            LogBuild<TypeOfAccommodation>();
            var typesOfAccommodations = BuildTypesOfAccommodations(eanEntities, CreatorId);
            var count = typesOfAccommodations.Length;
            LogBuilded(count);

            var typesOfAccommodationsRepository =
                FactoryOfRepositories.MappedEntities<TypeOfAccommodation>();

            if (count > 0)
            {
                LogSave<TypeOfAccommodation>();
                typesOfAccommodationsRepository.BulkSave(typesOfAccommodations);
                LogSaved<TypeOfAccommodation>();
            }

            LogBuild<LocalizedTypeOfAccommodation>();
            var localizedTypesOfAccommodations = BuildLocalizedTypesOfAccommodations(eanEntities,
                typesOfAccommodationsRepository.EanIdsToIds, DefaultLanguageId, CreatorId);
            count = localizedTypesOfAccommodations.Length;

            if (count <= 0) return;
            LogSave<LocalizedTypeOfAccommodation>();
            FactoryOfRepositories.Localized<LocalizedTypeOfAccommodation>().BulkSave(localizedTypesOfAccommodations);
            LogSaved<LocalizedTypeOfAccommodation>();

        }

        LocalizedTypeOfAccommodation[] BuildLocalizedTypesOfAccommodations(IEnumerable<PropertyType> propertyTypes,
            IReadOnlyDictionary<int, int> eanIdsToIds,
            int languageId,
            int creatorId
        )
        {

            var localizedTypesOfAccommodations = new Queue<LocalizedTypeOfAccommodation>();

            foreach (var propertyType in propertyTypes)
            {
                if (!eanIdsToIds.TryGetValue(propertyType.PropertyCategory, out var id)) continue;

                var localizedTypeOfAccommodation = new LocalizedTypeOfAccommodation
                {
                    Id = id,
                    LanguageId = languageId,
                    Name = propertyType.PropertyCategoryDesc,
                    CreatorId = creatorId
                };

                localizedTypesOfAccommodations.Enqueue(localizedTypeOfAccommodation);

            }
            return localizedTypesOfAccommodations.ToArray();
        }

        private static TypeOfAccommodation[] BuildTypesOfAccommodations(IEnumerable<PropertyType> propertyTypes,
            int creatorId
            )
        {
            return propertyTypes.Select(pt => new TypeOfAccommodation
            {
                EanId = pt.PropertyCategory,
                CreatorId = creatorId
            }).ToArray();
        }

    }
}
