using System;
using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.BLL;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;
using PointOfInterest = Olbrasoft.Travel.DTO.PointOfInterest;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class ParentRegionImporter : Importer<ParentRegion>
    {
        protected new readonly ParentRegionImportOption Option;
        protected readonly IFactoryOfRepositories Factory;

        protected ISubClassesFacade SubClassesFacade;
        protected IRegionsFacade RegionsFacade;
        protected IPointsOfInterestFacade PointsOfInterestFacade;

        public ParentRegionImporter(ParentRegionImportOption option, IFactoryOfRepositories factory) : base(option)
        {
            Option = option;
            Factory = factory;
            SubClassesFacade = Option.SubClassesFacade;
            RegionsFacade = Option.RegionsFacade;
            PointsOfInterestFacade = Option.PointsOfInterestFacade;
        }

        protected override void ImportBatch(ParentRegion[] parentRegions)
        {
            var continentRepository = Factory.BaseRegions<Continent>();
            ImportContinents(parentRegions, continentRepository, CreatorId);

            var eanRegionIdsToContinentIds = continentRepository.EanRegionIdsToIds;

            var localizedContinents = BuildLocalizedContinents(parentRegions, eanRegionIdsToContinentIds, CreatorId, DefaultLanguageId);
            ImportLocalizedContinents(localizedContinents, Factory.Travel<LocalizedContinent>(), DefaultLanguageId, Logger);

            var subClassesRepository = Factory.BaseNames<SubClass>();
            ImportSubClasses(parentRegions, subClassesRepository, CreatorId);

            var subClasses = subClassesRepository.NamesToIds;

            var typesOfRegions = Factory.BaseNames<TypeOfRegion>().NamesToIds;

            var regionsRepository = Factory.BaseRegions<Region>();

            ImportRegions(parentRegions, regionsRepository, subClasses, typesOfRegions, CreatorId);

            var regEanRegionIdsToIds = regionsRepository.EanRegionIdsToIds;

            ImportRegionsToRegions(parentRegions, regEanRegionIdsToIds, Factory.Travel<RegionToRegion>(), CreatorId);

            var pointsOfInterestRepository = Factory.BaseRegions<PointOfInterest>();

            ImportPointsOfInterest(parentRegions, pointsOfInterestRepository, subClasses, CreatorId);

            var poiEanRegionIdsToIds = pointsOfInterestRepository.EanRegionIdsToIds;

            ImportPointsOfInterestToPointsOfInterest(
                parentRegions, poiEanRegionIdsToIds, Factory.Travel<PointOfInterestToPointOfInterest>(), CreatorId);

            ImportPointsOfInterestToRegions(parentRegions, regEanRegionIdsToIds, poiEanRegionIdsToIds,
                Factory.Travel<PointOfInterestToRegion>(), CreatorId);


            //ImportLocalizedRegions(parentRegions, Factory.Travel<LocalizedRegion>(), regEanRegionIdsToIds, CreatorId, DefaultLanguageId);

            //ImportLocalizedPointsOfInterest(parentRegionsArray, LocalizedFacade, PointsOfInterestFacade, CreatorId, DefaultLanguageId);
        }


        private static void ImportLocalizedContinents(IReadOnlyCollection<LocalizedContinent> localizedContinents,
            ITravelRepository<LocalizedContinent> repository, int defaultLanguageId, ILoggingImports logger)
        {
            if (!repository.Exists(lc => lc.LanguageId == defaultLanguageId))
            {
                logger.Log($"Bulk Insert {localizedContinents.Count} LocalizedContinents.");
                repository.BulkInsert(localizedContinents);
                logger.Log($"LocalizedContinents Inserted.");
            }
            else
            {
                var storedLocalizedContinentIs =
                    new HashSet<int>(repository.FindAll(lc => lc.LanguageId == defaultLanguageId,
                        lc => lc.ContinentId));

                var forInsert = new List<LocalizedContinent>();
                var forUpdate = new List<LocalizedContinent>();

                foreach (var localizedContinent in localizedContinents)
                {
                    if (!storedLocalizedContinentIs.Contains(localizedContinent.ContinentId))
                    {
                        forInsert.Add(localizedContinent);
                    }
                    else
                    {
                        forUpdate.Add(localizedContinent);
                    }
                }

                if (forInsert.Count > 0)
                {
                    logger.Log($"Bulk Insert {forInsert.Count} LocalizedContinents.");
                    repository.BulkInsert(forInsert);
                    logger.Log($"LocalizedContinents Inserted.");
                }

                if (forUpdate.Count <= 0) return;
                logger.Log($"Bulk Update {forUpdate.Count} LocalizedContinents.");
                repository.BulkUpdate(forUpdate);
                logger.Log($"LocalizedContinents Updated.");
            }
        }


        private static LocalizedContinent[] BuildLocalizedContinents(IEnumerable<ParentRegion> parentRegions, IReadOnlyDictionary<long, int> eanRegionIdsToContinentIds, int creatorId, int defaultLanguageId)
        {
            const string s = "Continent";
            var localizedContinents = new Dictionary<long, LocalizedContinent>();

            foreach (var parentRegion in parentRegions)
            {
                if (parentRegion.ParentRegionType != s ||
                    !eanRegionIdsToContinentIds.TryGetValue(parentRegion.ParentRegionID, out var continentId)
                                                       ||
                    localizedContinents.ContainsKey(parentRegion.ParentRegionID)) continue;

                var localizedContinent = new LocalizedContinent
                {
                    ContinentId = continentId,
                    LanguageId = defaultLanguageId,
                    CreatorId = creatorId,
                    Name = parentRegion.ParentRegionName
                };

                if (parentRegion.ParentRegionName != parentRegion.ParentRegionNameLong)
                    localizedContinent.LongName = parentRegion.ParentRegionNameLong;

                localizedContinents.Add(parentRegion.ParentRegionID, localizedContinent);

            }

            return localizedContinents.Values.ToArray();
        }


        private void ImportContinents(IEnumerable<ParentRegion> parentRegions,
            IBaseRegionsRepository<Continent> continentsRepository,
            int creatorId)
        {
            const string s = "Continent";
            var regionIds = new HashSet<long>(continentsRepository.EanRegionIds);

            var continents = new Dictionary<long, Continent>();

            WriteLog("Continents Build.");

            foreach (var parentRegion in parentRegions)
            {
                if (parentRegion.ParentRegionType != s || regionIds.Contains(parentRegion.ParentRegionID) ||
                     continents.ContainsKey(parentRegion.ParentRegionID)) continue;

                continents.Add(parentRegion.ParentRegionID, new Continent
                {
                    EanRegionId = parentRegion.ParentRegionID,
                    CreatorId = creatorId
                });
            }

            var count = continents.Count;
            WriteLog($"Continents Builded:{count}.");
            if (count <= 0) return;

            WriteLog($"Continents Save.");
            continentsRepository.BulkInsert(continents.Values);
            WriteLog($"Continents Saved.");
        }

        private void ImportPointsOfInterestToRegions(
            IEnumerable<ParentRegion> parentRegions, IReadOnlyDictionary<long, int> regEanRegionIdsToIds,
            IReadOnlyDictionary<long, int> poiEanRegionIdsToIds, ITravelRepository<PointOfInterestToRegion> repository,
            int creatorId)
        {
            WriteLog("PointsOfInterestToRegions Build.");

            var pointsOfInterestToRegions = BuildPointsOfInterestToRegions(parentRegions,
                regEanRegionIdsToIds, poiEanRegionIdsToIds,
                repository.GetAll(ptr => new { PointOfInterestId = ptr.Id, RegionId = ptr.ToId })
                    .ToDictionary(k => k.PointOfInterestId, v => v.RegionId), creatorId);

            var count = pointsOfInterestToRegions.Length;
            WriteLog($"PointsOfInterestToRegions Builded:{count}.");

            if (count <= 0) return;
            WriteLog("PointsOfInterestToPointsOfInterest Save.");
            repository.BulkInsert(pointsOfInterestToRegions);
            WriteLog("PointsOfInterestToPointsOfInterest Saved.");
        }

        private void ImportPointsOfInterestToPointsOfInterest(IEnumerable<ParentRegion> parentRegions,
            IReadOnlyDictionary<long, int> poiEanRegionIdsToIds,
            ITravelRepository<PointOfInterestToPointOfInterest> repository, int creatorId)
        {
            WriteLog("PointsOfInterestToPointsOfInterest Build.");
            var pointsOfInterestToPointsOfInterest = BuildPointsOfInterestToPointsOfInterest(parentRegions,
                poiEanRegionIdsToIds,
                repository.GetAll(poi => new { PointOfInterestId = poi.Id, ParentPointOfInterestId = poi.ToId })
                    .ToDictionary(k => k.PointOfInterestId, v => v.ParentPointOfInterestId), creatorId);

            var count = pointsOfInterestToPointsOfInterest.Length;
            WriteLog($"PointsOfInterestToPointsOfInterest Builded:{count}.");

            if (count <= 0) return;
            WriteLog("PointsOfInterestToPointsOfInterest Save.");
            repository.BulkInsert(pointsOfInterestToPointsOfInterest);
            WriteLog("PointsOfInterestToPointsOfInterest Saved.");
        }

        private void ImportRegionsToRegions(IEnumerable<ParentRegion> parentRegions, IReadOnlyDictionary<long, int> eanRegionIdsToIds, ITravelRepository<RegionToRegion> repository, int creatorId)
        {
            WriteLog("RegionsToRegions Build.");
            var regionsToRegions = BuildRegionsToRegions(parentRegions, eanRegionIdsToIds,
                repository.GetAll(rtr => new { rtr.Id, rtr.ToId }).ToDictionary(k => k.Id, v => v.ToId), creatorId);
            var count = regionsToRegions.Length;
            WriteLog($"RegionsToRegions Builded:{count}.");

            if (count <= 0) return;
            WriteLog("RegionsToRegions Save.");
            repository.BulkInsert(regionsToRegions);
            WriteLog("RegionsToRegions Saved.");
        }

        private void ImportLocalizedPointsOfInterest(IEnumerable<ParentRegion> parentRegions, ILocalizedFacade facade,
            IPointsOfInterestFacade pointsOfInterestFacade, int creatorId, int defaultLanguageId)
        {
            WriteLog("LocalizedPoinsOfInterest Build.");
            var localizedPoinsOfInterest = BuildLocalizedPointsOfInterest(parentRegions,
                pointsOfInterestFacade.GetMappingEanRegionIdsToIds(true), creatorId, defaultLanguageId);
            var count = localizedPoinsOfInterest.Length;
            WriteLog($"LocalizedPoinsOfInterest Builded:{count}.");

            if (count <= 0) return;
            WriteLog("LocalizedPoinsOfInterest Save.");
            facade.BulkSave(localizedPoinsOfInterest);
            WriteLog("LocalizedPoinsOfInterest Saved.");
        }


        private void ImportLocalizedRegions(
            IEnumerable<ParentRegion> parentRegions, ITravelRepository<LocalizedRegion> repository,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds,
            int creatorId, int defaultLanguageId)
        {
            WriteLog("LocalizedRegions Build.");
            var localizedRegions = BuildLocalizedRegions(parentRegions, eanRegionIdsToIds,
                creatorId, defaultLanguageId);

            var count = localizedRegions.Length;
            WriteLog($"LocalizedRegions from Regions Builded:{count}.");

            if (count <= 0) return;
            WriteLog("LocalizedRegions Save.");
            repository.BulkInsert(localizedRegions);
            WriteLog("LocalizedRegions Saved.");
        }

        private void ImportPointsOfInterest(ParentRegion[] parentRegions, IBaseRegionsRepository<PointOfInterest> repository,
            IReadOnlyDictionary<string, int> subClasses, int creatorId)
        {
            ImportParentPointsOfInterest(parentRegions, repository, creatorId);

            WriteLog("PointsOfInterests from Regions Build.");

            var pointsOfInterest = BuildPointsOfInterest(parentRegions,
                repository.EanRegionIdsToIds,
                    subClasses, creatorId);

            var count = pointsOfInterest.Length;
            WriteLog($"PointsOfInterests from Regions Builded:{count} new.");

            if (count <= 0) return;
            WriteLog("PointsOfInterest Save.");
            repository.BulkSave(pointsOfInterest);
            WriteLog("PointsOfInterest Saved.");
        }

        private void ImportParentPointsOfInterest(IEnumerable<ParentRegion> parentRegions, IBaseRegionsRepository<PointOfInterest> repository, int creatorId)
        {
            WriteLog("PointsOfInterest from ParentRegions Build.");

            var pointsOfInterest = BuildPointsOfInterestFromParentRegions
                (parentRegions, repository.EanRegionIdsToIds, creatorId);

            var count = pointsOfInterest.Length;
            WriteLog($"PointsOfInterest from ParentRegions Builded:{count}.");

            if (count <= 0) return;
            WriteLog("PointsOfInterest Save.");
            repository.BulkSave(pointsOfInterest);
            WriteLog("PointsOfInterest Saved.");
        }

        private static PointOfInterestToRegion[] BuildPointsOfInterestToRegions(
            IEnumerable<ParentRegion> parentRegions,
            IReadOnlyDictionary<long, int> mappingRegionIdsToIds,
            IReadOnlyDictionary<long, int> mappingPointsOfInterestEanRegionIdsToIds,
            IReadOnlyDictionary<int, int> storedPointsOfInterestToRegions,
            int creatorId)
        {
            var pointsOfInterestToRegions = new HashSet<PointOfInterestToRegion>();
            foreach (var parentRegion in parentRegions)
            {

                if (!mappingRegionIdsToIds.TryGetValue(parentRegion.ParentRegionID, out var regionId)
                    ||
                    !mappingPointsOfInterestEanRegionIdsToIds.TryGetValue(parentRegion.RegionID, out var pointOfInterestId)
                ) continue;

                if (storedPointsOfInterestToRegions.TryGetValue(pointOfInterestId, out var storedParentRegionId) && storedParentRegionId == regionId)
                    continue;

                var regionToRegion = new PointOfInterestToRegion
                {
                    ToId = regionId,
                    Id = pointOfInterestId,
                    CreatorId = creatorId
                };

                if (!pointsOfInterestToRegions.Contains(regionToRegion))
                {
                    pointsOfInterestToRegions.Add(regionToRegion);
                }
            }

            return pointsOfInterestToRegions.ToArray();
        }


        private static PointOfInterestToPointOfInterest[] BuildPointsOfInterestToPointsOfInterest
        (
            IEnumerable<ParentRegion> parentRegions,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds,
            IReadOnlyDictionary<int, int> storedPointsOfInterestToPointsOfInterest,
            int creatorId
        )
        {
            var regionsToRegions = new HashSet<PointOfInterestToPointOfInterest>();
            foreach (var parentRegion in parentRegions)
            {
                if (!parentRegion.ParentRegionType.StartsWith("Point of Interest")
                    ||
                    !parentRegion.RegionType.StartsWith("Point of Interest"))
                    continue;

                if (!eanRegionIdsToIds.TryGetValue(parentRegion.ParentRegionID, out var parentPointOfInterestId)
                    ||
                    !eanRegionIdsToIds.TryGetValue(parentRegion.RegionID, out var pointOfInterestId)
                ) continue;

                if (storedPointsOfInterestToPointsOfInterest.TryGetValue(pointOfInterestId, out var storedParentRegionId)
                    &&
                    storedParentRegionId == parentPointOfInterestId)
                    continue;

                var pointOfInterestToPointOfInterest = new PointOfInterestToPointOfInterest()
                {
                    Id = pointOfInterestId,
                    ToId = parentPointOfInterestId,
                    CreatorId = creatorId

                };

                if (!regionsToRegions.Contains(pointOfInterestToPointOfInterest))
                {
                    regionsToRegions.Add(pointOfInterestToPointOfInterest);
                }
            }

            return regionsToRegions.ToArray();
        }

        private void ImportRegions(ParentRegion[] parentRegions, IBaseRegionsRepository<Region> repository, IReadOnlyDictionary<string, int> subClasses, IReadOnlyDictionary<string, int> typesOfRegions, int creatorId)
        {
            ImportParentRegions(parentRegions, repository, typesOfRegions, creatorId);

            WriteLog("Regions Build.");
            var regions = BuildRegions
             (parentRegions, typesOfRegions,
                repository.EanRegionIdsToIds, subClasses, creatorId);

            var count = regions.Length;
            WriteLog($"Regions Builded:{count}.");

            if (count <= 0) return;
            WriteLog("Regions Save.");
            repository.BulkSave(regions);
            WriteLog("Regions Saved.");
        }


        private void ImportParentRegions(IEnumerable<ParentRegion> parentRegions, IBaseRegionsRepository<Region> repository, IReadOnlyDictionary<string, int> storedTypesOfRegions, int creatorId)
        {
            WriteLog("ParentRegions Build.");
            var regions = BuildParentRegions(
                parentRegions, storedTypesOfRegions, repository.EanRegionIdsToIds, creatorId);

            var count = regions.Length;
            WriteLog($"ParentRegions Builded:{count} Regions.");

            if (count <= 0) return;
            WriteLog("ParentRegions Save.");
            repository.BulkSave(regions);
            WriteLog("ParenRegions Saved.");
        }


        private void ImportSubClasses(IEnumerable<ParentRegion> parentRegions, IBaseNamesRepository<SubClass> repository, int creatorId)
        {
            WriteLog("SubClasses Build.");
            var storedSubClasses =  new HashSet<string>(repository.Names);
            var subClasses = BuildSubClasses(parentRegions, storedSubClasses, creatorId);
            var count = subClasses.Length;
            WriteLog($"SubClasses Builded:{count}.");

            if (count <= 0) return;
            WriteLog("SubClasses Saving.");
            repository.BulkInsert(subClasses);
            WriteLog("SubClasses Saved.");
        }


        private static RegionToRegion[] BuildRegionsToRegions(IEnumerable<ParentRegion> parentRegions,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds, IDictionary<int, int> storedRegionsToRegions,
            int creatorId)
        {
            var regionsToRegions = new HashSet<RegionToRegion>();
            foreach (var parentRegion in parentRegions)
            {
                if (parentRegion.ParentRegionType.StartsWith("Point of Interest") || parentRegion.RegionType.StartsWith("Point of Interest"))
                    continue;

                if (!eanRegionIdsToIds.TryGetValue(parentRegion.ParentRegionID, out var parentRegionId)
                    ||
                    !eanRegionIdsToIds.TryGetValue(parentRegion.RegionID, out var regionId)
                )
                {
                    continue;
                }

                if (storedRegionsToRegions.TryGetValue(regionId, out var storedParentRegionId) && storedParentRegionId == parentRegionId)
                    continue;

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


        private static LocalizedPointOfInterest[] BuildLocalizedPointsOfInterest(IEnumerable<ParentRegion> parentRegions,
            IDictionary<long, int> mappingPointsOfInterestEanRegionIdsToIds, int creatorId, int defaultLanguageId)
        {
            var localizedPointsOfInterest = new Dictionary<int, LocalizedPointOfInterest>();
            foreach (var parentRegion in parentRegions)
            {
                LocalizedPointOfInterest localizedPointOfInterest;
                if (mappingPointsOfInterestEanRegionIdsToIds.TryGetValue(parentRegion.ParentRegionID, out var pointOfInterestId))
                {
                    if (!localizedPointsOfInterest.ContainsKey(pointOfInterestId))
                    {
                        localizedPointOfInterest = new LocalizedPointOfInterest()
                        {
                            PointOfInterestId = pointOfInterestId,
                            Name = parentRegion.ParentRegionName,
                            CreatorId = creatorId,
                            LanguageId = defaultLanguageId,
                            DateAndTimeOfCreation = DateTime.Now
                        };

                        if (parentRegion.ParentRegionName.Trim() != parentRegion.ParentRegionNameLong.Trim())
                        {
                            localizedPointOfInterest.LongName = parentRegion.ParentRegionNameLong;
                        }

                        localizedPointsOfInterest.Add(pointOfInterestId, localizedPointOfInterest);
                    }
                }

                if (!mappingPointsOfInterestEanRegionIdsToIds.TryGetValue(parentRegion.RegionID, out pointOfInterestId)) continue;
                if (localizedPointsOfInterest.ContainsKey(pointOfInterestId)) continue;
                localizedPointOfInterest = new LocalizedPointOfInterest()
                {
                    PointOfInterestId = pointOfInterestId,
                    Name = parentRegion.RegionName,
                    CreatorId = creatorId,
                    LanguageId = defaultLanguageId
                };

                if (parentRegion.RegionName.Trim() != parentRegion.RegionNameLong.Trim())
                {
                    localizedPointOfInterest.LongName = parentRegion.RegionNameLong;
                }

                localizedPointsOfInterest.Add(pointOfInterestId, localizedPointOfInterest);
            }

            return localizedPointsOfInterest.Values.ToArray();
        }

        private static LocalizedRegion[] BuildLocalizedRegions(IEnumerable<ParentRegion> parentRegions,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds, int creatorId, int defaultLanguageId)
        {
            var localizedRegions = new Dictionary<int, LocalizedRegion>();
            foreach (var parentRegion in parentRegions)
            {
                LocalizedRegion localizedRegion;
                if (eanRegionIdsToIds.TryGetValue(parentRegion.ParentRegionID, out var regionId))
                {
                    if (!localizedRegions.ContainsKey(regionId))
                    {
                        localizedRegion = new LocalizedRegion()
                        {
                            RegionId = regionId,
                            Name = parentRegion.ParentRegionName,
                            CreatorId = creatorId,
                            LanguageId = defaultLanguageId
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
                localizedRegion = new LocalizedRegion()
                {
                    RegionId = regionId,
                    Name = parentRegion.RegionName,
                    CreatorId = creatorId,
                    LanguageId = defaultLanguageId
                };

                if (parentRegion.RegionName.Trim() != parentRegion.RegionNameLong.Trim())
                {
                    localizedRegion.LongName = parentRegion.RegionNameLong;
                }

                localizedRegions.Add(regionId, localizedRegion);
            }

            return localizedRegions.Values.ToArray();
        }


        private static PointOfInterest[] BuildPointsOfInterest(IEnumerable<ParentRegion> parentRegions,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds, IReadOnlyDictionary<string, int> subClasses, int creatorId)
        {
            var pointsOfInterest = new Dictionary<long, PointOfInterest>();
            foreach (var sourcePointOfInterest in parentRegions)
            {
                if (!sourcePointOfInterest.RegionType.StartsWith("Point of Interest")) continue;
                if (pointsOfInterest.ContainsKey(sourcePointOfInterest.RegionID)) continue;

                var pointOfInterest = new PointOfInterest
                {
                    EanRegionId = sourcePointOfInterest.RegionID,
                    CreatorId = creatorId
                };

                if (eanRegionIdsToIds.TryGetValue(sourcePointOfInterest.RegionID, out var id))
                {
                    pointOfInterest.Id = id;
                }

                if (subClasses.TryGetValue(sourcePointOfInterest.SubClass, out var subClassId))
                {
                    pointOfInterest.SubClassId = subClassId;
                }

                pointOfInterest.Shadow = sourcePointOfInterest.RegionType.EndsWith("Shadow");
                pointsOfInterest.Add(sourcePointOfInterest.RegionID, pointOfInterest);
            }

            return pointsOfInterest.Values.ToArray();
        }

        private static PointOfInterest[] BuildPointsOfInterestFromParentRegions(IEnumerable<ParentRegion> parentRegions,
            IReadOnlyDictionary<long, int> mappingEanRegionIdsToIds, int creatorId)
        {

            var pointsOfInterest = new Dictionary<long, PointOfInterest>();
            foreach (var sourcePointOfInterest in parentRegions)
            {
                if (!sourcePointOfInterest.ParentRegionType.StartsWith("Point of Interest")) continue;
                if (pointsOfInterest.ContainsKey(sourcePointOfInterest.ParentRegionID)) continue;

                var pointOfInterest = new PointOfInterest
                {
                    EanRegionId = sourcePointOfInterest.ParentRegionID,
                    CreatorId = creatorId,
                };

                if (mappingEanRegionIdsToIds.TryGetValue(sourcePointOfInterest.ParentRegionID, out var id))
                {
                    pointOfInterest.Id = id;
                }

                pointOfInterest.Shadow = sourcePointOfInterest.ParentRegionType.EndsWith("Shadow");
                pointsOfInterest.Add(sourcePointOfInterest.ParentRegionID, pointOfInterest);
            }
            return pointsOfInterest.Values.ToArray();
        }

        private static Region[] BuildRegions(IEnumerable<ParentRegion> parentRegions, IReadOnlyDictionary<string, int> typesOfRegions,
             IReadOnlyDictionary<long, int> eanRegionIdsToIds, IReadOnlyDictionary<string, int> storedSubClasses, int creatorId)
        {
            var regions = new Dictionary<long, Region>();
            foreach (var parentRegion in parentRegions)
            {
                if (regions.ContainsKey(parentRegion.RegionID)) continue;
                if (!typesOfRegions.TryGetValue(parentRegion.RegionType, out var typeOfRegionId)) continue;
                var region = new Region
                {
                    EanRegionId = parentRegion.RegionID,
                    TypeOfRegionId = typeOfRegionId
                };

                if (eanRegionIdsToIds.TryGetValue(parentRegion.RegionID, out var id))
                {
                    region.Id = id;
                }
                else
                {
                    region.CreatorId = creatorId;
                }

                if (!string.IsNullOrEmpty(parentRegion.SubClass))
                {
                    if (storedSubClasses.TryGetValue(parentRegion.SubClass, out var subClassId))
                    {
                        region.SubClassId = subClassId;
                    }
                }

                regions.Add(parentRegion.RegionID, region);
            }
            return regions.Values.ToArray();
        }

        private static Region[] BuildParentRegions(IEnumerable<ParentRegion> parentRegions, IReadOnlyDictionary<string, int> typesOfRegions,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds, int creatorId)
        {
            var regions = new Dictionary<long, Region>();
            foreach (var parentRegion in parentRegions)
            {
                if (regions.ContainsKey(parentRegion.ParentRegionID)) continue;
                if (!typesOfRegions.TryGetValue(parentRegion.ParentRegionType, out var typeOfRegionId)) continue;
                var region = new Region
                {
                    EanRegionId = parentRegion.ParentRegionID,
                    TypeOfRegionId = typeOfRegionId
                };

                if (eanRegionIdsToIds.TryGetValue(parentRegion.ParentRegionID, out var id))
                {
                    region.Id = id;
                }
                else
                {
                    region.CreatorId = creatorId;
                }

                regions.Add(parentRegion.ParentRegionID, region);
            }
            return regions.Values.ToArray();
        }

        private static SubClass[] BuildSubClasses(IEnumerable<ParentRegion> entities, HashSet<string> subClasses, int creatorId)
        {
            var subClassesOfRegionsToImport = new Dictionary<string, SubClass>();

            foreach (var parentRegion in entities)
            {
                if (!string.IsNullOrEmpty(parentRegion.SubClass) &&
                    !subClasses.Contains(parentRegion.SubClass.ToLower()) &&
                    !subClassesOfRegionsToImport.ContainsKey(parentRegion.SubClass.ToLower()))
                {
                    subClassesOfRegionsToImport.Add(
                        parentRegion.SubClass,
                        new SubClass
                        {
                            Name = parentRegion.SubClass.ToLower(),
                            CreatorId = creatorId
                        }
                    );
                }
            }
            return subClassesOfRegionsToImport.Values.ToArray();
        }
    }


}
