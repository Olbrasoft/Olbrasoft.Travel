using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class RegionsImporter : Importer
    {
        private readonly IParserFactory _parserFactory;
        protected IParser<ParentRegion> Parser;
        protected Queue<ParentRegion> ParentRegions = new Queue<ParentRegion>();


        public RegionsImporter(IProvider provider, IParserFactory parserFactory, IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger)
            : base(provider, factoryOfRepositories, sharedProperties, logger)
        {
            _parserFactory = parserFactory;
        }

        protected override void RowLoaded(string[] items)
        {
            ParentRegions.Enqueue(Parser.Parse(items));
        }

        public override void Import(string path)
        {
            Parser = _parserFactory.Create<ParentRegion>(Provider.GetFirstLine(path));

            LoadData(path);

            var parentRegions = ParentRegions.ToArray();

            const int batchSize = 300000;

            var eanRegionIdsToIds = ImportRegions(parentRegions, FactoryOfRepositories.Regions(), batchSize, CreatorId,
                out var subClassNames);

            var subClasses = ImportSubClasses(FactoryOfRepositories.BaseNames<SubClass>(), subClassNames, CreatorId);

            ImportRegionsToTypes(parentRegions, FactoryOfRepositories.ManyToMany<RegionToType>(), batchSize,
                eanRegionIdsToIds, FactoryOfRepositories.BaseNames<TypeOfRegion>().NamesToIds, subClasses, CreatorId);

            ImportLocalized(parentRegions, FactoryOfRepositories.Localized<LocalizedRegion>(), batchSize, eanRegionIdsToIds, DefaultLanguageId, CreatorId);

            ImportRegionsToRegions(parentRegions, FactoryOfRepositories.ManyToMany<RegionToRegion>(), batchSize, eanRegionIdsToIds, CreatorId);

            ParentRegions = null;
        }
        
        private void ImportRegionsToRegions(IEnumerable<ParentRegion> parentRegions,
            IManyToManyRepository<RegionToRegion> repository,
            int batchSize,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds,
            int creatorId)
        {

            LogBuild<RegionToRegion>();
            var regionsToRegions = BuildRegionsToRegions(
                parentRegions,
                eanRegionIdsToIds,
                creatorId
            );

            var count = regionsToRegions.Length;

            LogBuilded(count);

            if (count <= 0) return;
            LogSave<RegionToRegion>();
            repository.BulkSave(regionsToRegions, batchSize);
            LogSaved<RegionToRegion>();
        }

        private static RegionToRegion[] BuildRegionsToRegions(IEnumerable<ParentRegion> parentRegions,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds,
            int creatorId)
        {
            var regionsToRegions = new HashSet<RegionToRegion>();
            foreach (var parentRegion in parentRegions)
            {
                if (!eanRegionIdsToIds.TryGetValue(parentRegion.ParentRegionID, out var parentRegionId)
                    ||
                    !eanRegionIdsToIds.TryGetValue(parentRegion.RegionID, out var regionId)
                )
                {
                    continue;
                }

                var regionToRegion = new RegionToRegion()
                {
                    Id = regionId,
                    ToId = parentRegionId,
                    CreatorId = creatorId

                };

                if (!regionsToRegions.Contains(regionToRegion))
                {
                    regionsToRegions.Add(regionToRegion);
                }
            }

            return regionsToRegions.ToArray();
        }

        private void ImportLocalized<T>(
            IEnumerable<ParentRegion> parentRegions,
            ILocalizedRepository<T> repository,
            int batchSize,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            int languageId,
            int creatorId

        ) where T : LocalizedRegion, new()
        {
            LogBuild<T>();
            var localizedEntities = BuildLocalizedRegions<T>(parentRegions, eanIdsToIds, languageId, creatorId);
            var count = localizedEntities.Length;
            LogBuilded(count);

            if (count <= 0) return;

            LogSave<T>();
            repository.BulkSave(localizedEntities, batchSize);
            LogSaved<T>();
        }

        private static T[] BuildLocalizedRegions<T>(
            IEnumerable<ParentRegion> parentRegions,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds,
            int languageId,
            int creatorId
            )
            where T : LocalizedRegion, new()
        {
            var localizedRegions = new Dictionary<int, T>();
            foreach (var parentRegion in parentRegions)
            {
                T localizedRegion;
                if (eanRegionIdsToIds.TryGetValue(parentRegion.ParentRegionID, out var regionId))
                {
                    if (!localizedRegions.ContainsKey(regionId))
                    {
                        localizedRegion = new T
                        {
                            Id = regionId,
                            Name = parentRegion.ParentRegionName,
                            CreatorId = creatorId,
                            LanguageId = languageId
                        };

                        if (parentRegion.ParentRegionName.Trim() != parentRegion.ParentRegionNameLong.Trim())
                        {
                            localizedRegion.LongName = parentRegion.ParentRegionNameLong;
                        }

                        localizedRegions.Add(regionId, localizedRegion);
                    }
                }

                if (!eanRegionIdsToIds.TryGetValue(parentRegion.RegionID, out regionId)) continue;
                if (localizedRegions.ContainsKey(regionId)) continue;
                localizedRegion = new T
                {
                    Id = regionId,
                    Name = parentRegion.RegionName,
                    CreatorId = creatorId,
                    LanguageId = languageId
                };

                if (parentRegion.RegionName.Trim() != parentRegion.RegionNameLong.Trim())
                {
                    localizedRegion.LongName = parentRegion.RegionNameLong;
                }

                localizedRegions.Add(regionId, localizedRegion);
            }

            return localizedRegions.Values.ToArray();
        }

        private void ImportRegionsToTypes(IEnumerable<ParentRegion> parentRegions,
            IManyToManyRepository<RegionToType> repository,
            int batchSize,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            IReadOnlyDictionary<string, int> namesToTypeIds,
            IReadOnlyDictionary<string, int> namesToSubClassIds,
            int creatorId)
        {
            LogBuild<RegionToType>();
            var regionsTotypes =
                BuildRegionsToTypes(parentRegions, eanIdsToIds, namesToTypeIds, namesToSubClassIds, creatorId);
            var count = regionsTotypes.Length;
            LogBuilded(count);

            LogSave<RegionToType>();
            repository.BulkSave(regionsTotypes, batchSize);
            LogSaved<RegionToType>();
        }

        private static RegionToType[] BuildRegionsToTypes(IEnumerable<ParentRegion> parentRegions,
          IReadOnlyDictionary<long, int> eanIdsToIds,
          IReadOnlyDictionary<string, int> namesToTypeIds,
          IReadOnlyDictionary<string, int> namesToSubClassIds,
          int creatorId
      )
        {
            var regionIds = new HashSet<long>();
            var regionsToTypes = new Queue<RegionToType>();

            foreach (var parentRegion in parentRegions)
            {
                if (!regionIds.Contains(parentRegion.RegionID) &&
                     eanIdsToIds.TryGetValue(parentRegion.RegionID, out var id) &&
                     namesToTypeIds.TryGetValue(parentRegion.RegionType, out var toId))
                {
                    var regionToType = new RegionToType
                    {
                        Id = id,
                        ToId = toId,
                        CreatorId = creatorId
                    };

                    var subClassName = GetSubClassName(parentRegion.SubClass);

                    if (!string.IsNullOrEmpty(subClassName) && namesToSubClassIds.TryGetValue(parentRegion.SubClass, out var subClassId))
                    {
                        regionToType.SubClassId = subClassId;
                    }

                    regionsToTypes.Enqueue(regionToType);
                    regionIds.Add(parentRegion.RegionID);
                }

                if (regionIds.Contains(parentRegion.ParentRegionID) ||
                    !eanIdsToIds.TryGetValue(parentRegion.ParentRegionID, out var pId) ||
                    !namesToTypeIds.TryGetValue(parentRegion.ParentRegionType, out var pToId)) continue;
                {
                    var regionToType = new RegionToType
                    {
                        Id = pId,
                        ToId = pToId,
                        CreatorId = creatorId
                    };

                    regionsToTypes.Enqueue(regionToType);
                    regionIds.Add(parentRegion.ParentRegionID);
                }
            }

            return regionsToTypes.ToArray();
        }



        private IReadOnlyDictionary<string, int> ImportSubClasses(ITypesRepository<SubClass> repository, IEnumerable<string> subClassesNames, int creatorId)
        {
            LogBuild<SubClass>();
            var subClasses = subClassesNames.Select(s => new SubClass { Name = s, CreatorId = creatorId }).ToArray();
            var count = subClasses.Length;
            LogBuilded(count);

            if (count <= 0) return repository.NamesToIds;
            LogSave<SubClass>();
            repository.BulkSave(subClasses);
            LogSaved<SubClass>();

            return repository.NamesToIds;
        }


        private IReadOnlyDictionary<long, int> ImportRegions(ParentRegion[] parentRegions,
            IRegionsRepository repository,
            int batchSize,
            int creatorId,
            out HashSet<string> subClassNames
        )
        {
            ImportParentRegions(parentRegions, repository, batchSize, creatorId);

            LogBuild<Region>();

            var regions = BuildRegions(parentRegions, creatorId, out subClassNames);
            var count = regions.Length;
            LogBuilded(count);

            if (count <= 0) return repository.EanIdsToIds;

            LogSave<Region>();
            repository.BulkSave(regions, batchSize, p => p.Coordinates, p => p.CenterCoordinates);
            LogSaved<Region>();

            return repository.EanIdsToIds;
        }

        private void ImportParentRegions(IEnumerable<ParentRegion> parentRegions,
            IRegionsRepository repository,
            int batchSize,
            int creatorId
        )
        {
            WriteLog("ParentRegions Build.");
            var regions = BuildParentRegions(
                parentRegions, creatorId);

            var count = regions.Length;
            WriteLog($"ParentRegions Builded:{count} Regions.");

            if (count <= 0) return;
            WriteLog("ParentRegions Save.");
            repository.BulkSave(regions, batchSize, p => p.Coordinates, p => p.CenterCoordinates);
            WriteLog("ParenRegions Saved.");
        }

        private static Region[] BuildRegions(IEnumerable<ParentRegion> parentRegions,
            int creatorId,
            out HashSet<string> subClassesNames
        )
        {
            subClassesNames = new HashSet<string>();
            var regions = new Dictionary<long, Region>();

            foreach (var parentRegion in parentRegions)
            {
                if (regions.ContainsKey(parentRegion.RegionID)) continue;

                var region = new Region
                {
                    EanId = parentRegion.RegionID,
                    CreatorId = creatorId
                };

                var subClassName = GetSubClassName(parentRegion.SubClass);

                if (!string.IsNullOrEmpty(subClassName) && !subClassesNames.Contains(subClassName))
                {
                    subClassesNames.Add(subClassName);
                }

                regions.Add(parentRegion.RegionID, region);
            }
            return regions.Values.ToArray();
        }

        private static Region[] BuildParentRegions(IEnumerable<ParentRegion> parentRegions,
            int creatorId
        )
        {
            var regions = new Dictionary<long, Region>();
            foreach (var parentRegion in parentRegions)
            {
                if (regions.ContainsKey(parentRegion.ParentRegionID)) continue;

                var region = new Region
                {
                    EanId = parentRegion.ParentRegionID,
                    CreatorId = creatorId
                };

                regions.Add(parentRegion.ParentRegionID, region);
            }
            return regions.Values.ToArray();
        }
    }
}