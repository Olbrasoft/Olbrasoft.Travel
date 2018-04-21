using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration.Interceptor;
using Olbrasoft.Travel.DTO;
using Description = Olbrasoft.Travel.EAN.DTO.Property.Description;

namespace Olbrasoft.Travel.EAN.Import
{
    class DescriptionsBatchImporter:BatchImporter<Travel.EAN.DTO.Property.Description>
    {
        public DescriptionsBatchImporter(ImportOption option) : base(option)
        {
        }

        public override void ImportBatch(Description[] eanEntities)
        {

            const string general = "General";
            var typesOfDescriptionsRepository = FactoryOfRepositories.BaseNames<TypeOfDescription>();

            if (!typesOfDescriptionsRepository.NamesToIds.ContainsKey(general))
            {
                typesOfDescriptionsRepository.Add(new TypeOfDescription(){ Name = general,CreatorId = CreatorId});
            }

            var typeOfDescriptionId = typesOfDescriptionsRepository.GetId(general);
            
            LogBuild<Description>();
            var descriptions = BuildDescriptions(eanEntities,
                FactoryOfRepositories.MappedEntities<Accommodation>().EanIdsToIds,
                FactoryOfRepositories.Languages().EanLanguageCodesToIds, typeOfDescriptionId, CreatorId);
            var count = descriptions.Length;
            LogBuilded(count);

            if (count > 0)
            {
                LogSave<Description>();
                FactoryOfRepositories.Descriptions().BulkSave(descriptions);
                LogSaved<Description>();
            }

        }



        Travel.DTO.Description[] BuildDescriptions(IEnumerable<Description> eanEnities,
        IReadOnlyDictionary<int,int> eanIdsToIds,
        IReadOnlyDictionary<string,int> eanLanuageCodesToIds,
        int typeOfdescriptionId,
        int creatorId
        )
        {
            var descriptions = new Queue<Travel.DTO.Description>();

            foreach (var eanEnity in eanEnities)
            {
                if(!eanIdsToIds.TryGetValue(eanEnity.EANHotelID,out var accommodationId) || !eanLanuageCodesToIds.TryGetValue(eanEnity.LanguageCode,out var languageId) ) continue;    
                
                var description = new Travel.DTO.Description
                {
                    AccommodationId = accommodationId,
                    TypeOfDescriptionId = typeOfdescriptionId,
                    LanguageId = languageId,
                    Text = eanEnity.PropertyDescription,
                    CreatorId = creatorId
                    
                };

               descriptions.Enqueue(description);
            }

            return descriptions.ToArray();
        }

    }
}
