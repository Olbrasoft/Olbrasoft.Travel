using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class ContinentsImporter : BaseImporter<ParentContinet>
    {
        public ContinentsImporter(ImportOption option) : base(option)
        {
        }


        public override void Import(string path)
        {
            var parser = CreateParser(path);
            var parentContinents = parser.Parse(Provider.GetAllLines(path).Skip(1).Take(50000)).ToArray();

            var repository = FactoryOfRepositories.Geo<Continent>();
            ImportContinents(parentContinents, repository, CreatorId);

            var eanIdsToIds = repository.EanIdsToIds;
            ImportLocalizedContinents(parentContinents, eanIdsToIds, FactoryOfRepositories.Localized<LocalizedContinent>(), CreatorId, DefaultLanguageId);
        }

        private void ImportLocalizedContinents(IEnumerable<ParentContinet> parentContinents,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            IBaseRepository<LocalizedContinent, int, int> repository,
            int creatorId,
            int languageId)
        {

            LogBuild<LocalizedContinent>();
            var localizedContinents = BuildLocalizedContinents(parentContinents, eanIdsToIds, creatorId, languageId);
            LogBuilded(localizedContinents.Length);

            LogSave<LocalizedContinent>();
            repository.BulkSave(localizedContinents);
            LogSaved<LocalizedContinent>();
        }


        private void ImportContinents(IEnumerable<ParentContinet> parentContinents,
            IMapToPartnersRepository<Continent, long> repository,
            int creatorId)
        {
            LogBuild<Continent>();
            var continents = BuildContinents(parentContinents, creatorId);

            var count = continents.Length;
            LogBuilded(count);

            if (count <= 0) return;
            LogSaved<Continent>();
            repository.BulkSave(continents);
            LogSaved<Continent>();
        }


        private static Continent[] BuildContinents(IEnumerable<ParentContinet> parentContinents, int creatorId)
        {
            const string s = "Continent";

            var continents = new Dictionary<long, Continent>();
            

            foreach (var parentRegion in parentContinents)
            {
                if (parentRegion.ParentRegionType != s || continents.ContainsKey(parentRegion.ParentRegionID)) continue;

                continents.Add(parentRegion.ParentRegionID, new Continent
                {
                    EanId = parentRegion.ParentRegionID,
                    CreatorId = creatorId
                });
            }

            return continents.Values.ToArray();
        }


        private static LocalizedContinent[] BuildLocalizedContinents(IEnumerable<ParentContinet> parentContinents,
            IReadOnlyDictionary<long, int> eanRegionIdsToContinentIds,
            int creatorId,
            int defaultLanguageId)
        {
            const string s = "Continent";
            var localizedContinents = new Dictionary<long, LocalizedContinent>();

            foreach (var parentContinet in parentContinents)
            {
                if (parentContinet.ParentRegionType != s ||
                    !eanRegionIdsToContinentIds.TryGetValue(parentContinet.ParentRegionID, out var continentId)
                    ||
                    localizedContinents.ContainsKey(parentContinet.ParentRegionID)) continue;

                var localizedContinent = new LocalizedContinent
                {
                    Id = continentId,
                    LanguageId = defaultLanguageId,
                    CreatorId = creatorId,
                    Name = parentContinet.ParentRegionName
                };

                if (parentContinet.ParentRegionName != parentContinet.ParentRegionNameLong)
                    localizedContinent.LongName = parentContinet.ParentRegionNameLong;

                localizedContinents.Add(parentContinet.ParentRegionID, localizedContinent);

            }

            return localizedContinents.Values.ToArray();
        }

    }
}