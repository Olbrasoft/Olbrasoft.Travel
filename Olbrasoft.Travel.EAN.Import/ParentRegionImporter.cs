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
   
       
        public ParentRegionImporter(ImportOption option) : base(option)
        {
            
        }

        protected override void ImportBatch(ParentRegion[] parentRegions)
        {
            var continentRepository = FactoryOfRepositories.BaseRegions<Continent>();
            ImportContinents(parentRegions, continentRepository, CreatorId);

            var eanRegionIdsToIds = continentRepository.EanRegionIdsToIds;

            Logger.Log($"LocalizedContinents Build.");
            var localizedContinents = BuildLocalizedContinents(parentRegions, eanRegionIdsToIds, CreatorId, DefaultLanguageId);
            Logger.Log($"LocalizedContinents builded count:{localizedContinents.Length}");

            Logger.Log("LocalizedContinents Save");
            FactoryOfRepositories.Localized<LocalizedContinent>().BulkSave(localizedContinents);
            Logger.Log("LocalizedContinents Saved");

            //BulkSaveLocalized(localizedContinents, FactoryOfRepositories.Localized<LocalizedContinent>(), DefaultLanguageId, Logger);

            var subClassesRepository = FactoryOfRepositories.BaseNames<SubClass>();
            ImportSubClasses(parentRegions, subClassesRepository, CreatorId);

            var subClasses = subClassesRepository.NamesToIds;

            var typesOfRegions = FactoryOfRepositories.BaseNames<TypeOfRegion>().NamesToIds;

            var regionsRepository = FactoryOfRepositories.BaseRegions<Region>();

            ImportRegions(parentRegions, regionsRepository, subClasses, typesOfRegions, CreatorId);

            var regEanRegionIdsToIds = regionsRepository.EanRegionIdsToIds;

            ImportRegionsToRegions(parentRegions, regEanRegionIdsToIds, FactoryOfRepositories.ManyToMany<RegionToRegion>(), CreatorId);

            var pointsOfInterestRepository = FactoryOfRepositories.BaseRegions<PointOfInterest>();

            ImportPointsOfInterest(parentRegions, pointsOfInterestRepository, subClasses, CreatorId);

            var poiEanRegionIdsToIds = pointsOfInterestRepository.EanRegionIdsToIds;

            ImportPointsOfInterestToPointsOfInterest(
                parentRegions, poiEanRegionIdsToIds, FactoryOfRepositories.ManyToMany<PointOfInterestToPointOfInterest>(), CreatorId);

            ImportPointsOfInterestToRegions(parentRegions, regEanRegionIdsToIds, poiEanRegionIdsToIds,
                FactoryOfRepositories.ManyToMany<PointOfInterestToRegion>(), CreatorId);

            Logger.Log($"Load EanRegionIdsToIds from Regions");
            eanRegionIdsToIds = regionsRepository.EanRegionIdsToIds;
            Logger.Log($"Ids regions loaded count:{eanRegionIdsToIds.Count}");

            Logger.Log($"Build LocalizedRegions");
            var localizedRegions = BuildLocalizedRegions<LocalizedRegion>(parentRegions, eanRegionIdsToIds, CreatorId, DefaultLanguageId);
            Logger.Log($"LocalizedRegions builded count:{localizedRegions.Length}");

            //BulkSaveLocalized(localizedRegions, FactoryOfRepositories.Localized<LocalizedRegion>(), DefaultLanguageId, Logger);
            Logger.Log("LocalizedRegions Save");
            FactoryOfRepositories.Localized<LocalizedRegion>().BulkSave(localizedRegions);
            Logger.Log("LocalizedRegions Saved.");
            
            eanRegionIdsToIds = pointsOfInterestRepository.EanRegionIdsToIds;

            Logger.Log($"LocalizedPointsOfInterest Build.");
            var localizedPointsOfInterest = BuildLocalizedRegions<LocalizedPointOfInterest>(parentRegions, eanRegionIdsToIds, CreatorId, DefaultLanguageId);
            Logger.Log($"LocalizedPointsOfInterest builded count:{localizedPointsOfInterest.Length}");

            Logger.Log($"LocalizedPointsOfInterest Save.");
            FactoryOfRepositories.Localized<LocalizedPointOfInterest>().BulkSave(localizedPointsOfInterest);
            Logger.Log($"LocalizedPointsOfInterest Saved.");

            //BulkSaveLocalized(localizedPointsOfInterest, FactoryOfRepositories.Localized<LocalizedPointOfInterest>(), DefaultLanguageId, Logger);

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
                    Id = continentId,
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
            var eanRegionIds = new HashSet<long>(continentsRepository.EanRegionIds);

            var continents = new Dictionary<long, Continent>();

            WriteLog("Continents Build.");

            foreach (var parentRegion in parentRegions)
            {
                if (parentRegion.ParentRegionType != s || eanRegionIds.Contains(parentRegion.ParentRegionID) ||
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
            continentsRepository.BulkSave(continents.Values);
            WriteLog($"Continents Saved.");
        }

        private void ImportPointsOfInterestToRegions(
            IEnumerable<ParentRegion> parentRegions, IReadOnlyDictionary<long, int> regEanRegionIdsToIds,
            IReadOnlyDictionary<long, int> poiEanRegionIdsToIds, IManyToManyRepository<PointOfInterestToRegion> repository,
            int creatorId)
        {
            WriteLog("PointsOfInterestToRegions Build.");

            var pointsOfInterestToRegions = BuildPointsOfInterestToRegions(parentRegions,
                regEanRegionIdsToIds, poiEanRegionIdsToIds,
                repository.IdsToToIds, creatorId);

            var count = pointsOfInterestToRegions.Length;
            WriteLog($"PointsOfInterestToRegions Builded:{count}.");

            if (count <= 0) return;
            WriteLog("PointsOfInterestToPointsOfInterest Save.");
            repository.BulkInsert(pointsOfInterestToRegions);
            WriteLog("PointsOfInterestToPointsOfInterest Saved.");
        }

        private void ImportPointsOfInterestToPointsOfInterest(IEnumerable<ParentRegion> parentRegions,
            IReadOnlyDictionary<long, int> poiEanRegionIdsToIds,
            IManyToManyRepository<PointOfInterestToPointOfInterest> repository, int creatorId)
        {
            WriteLog("PointsOfInterestToPointsOfInterest Build.");
            var pointsOfInterestToPointsOfInterest = BuildPointsOfInterestToPointsOfInterest(parentRegions,
                poiEanRegionIdsToIds,
                repository.IdsToToIds, creatorId);

            var count = pointsOfInterestToPointsOfInterest.Length;
            WriteLog($"PointsOfInterestToPointsOfInterest Builded:{count}.");

            if (count <= 0) return;
            WriteLog("PointsOfInterestToPointsOfInterest Save.");
            repository.BulkInsert(pointsOfInterestToPointsOfInterest);
            WriteLog("PointsOfInterestToPointsOfInterest Saved.");
        }

        private void ImportRegionsToRegions(IEnumerable<ParentRegion> parentRegions, IReadOnlyDictionary<long, int> eanRegionIdsToIds, IManyToManyRepository<RegionToRegion> repository, int creatorId)
        {
            WriteLog("RegionsToRegions Build.");
            var regionsToRegions = BuildRegionsToRegions(parentRegions, eanRegionIdsToIds,
                repository.IdsToToIds, creatorId);
            var count = regionsToRegions.Length;
            WriteLog($"RegionsToRegions Builded:{count}.");

            if (count <= 0) return;
            WriteLog("RegionsToRegions Save.");
            repository.BulkInsert(regionsToRegions);
            WriteLog("RegionsToRegions Saved.");
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
            var storedSubClasses = new HashSet<string>(repository.Names);
            var subClasses = BuildSubClasses(parentRegions, storedSubClasses, creatorId);
            var count = subClasses.Length;
            WriteLog($"SubClasses Builded:{count}.");

            if (count <= 0) return;
            WriteLog("SubClasses Saving.");
            repository.BulkSave(subClasses);
            WriteLog("SubClasses Saved.");
        }


        private static RegionToRegion[] BuildRegionsToRegions(IEnumerable<ParentRegion> parentRegions,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds, IReadOnlyDictionary<int, int> storedRegionsToRegions,
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


        //private static LocalizedPointOfInterest[] BuildLocalizedPointsOfInterest(IEnumerable<ParentRegion> parentRegions,
        //    IReadOnlyDictionary<long, int> mappingPointsOfInterestEanRegionIdsToIds, int creatorId, int defaultLanguageId)
        //{
        //    var localizedPointsOfInterest = new Dictionary<int, LocalizedPointOfInterest>();
        //    foreach (var parentRegion in parentRegions)
        //    {
        //        LocalizedPointOfInterest localizedPointOfInterest;
        //        if (mappingPointsOfInterestEanRegionIdsToIds.TryGetValue(parentRegion.ParentRegionID, out var pointOfInterestId))
        //        {
        //            if (!localizedPointsOfInterest.ContainsKey(pointOfInterestId))
        //            {
        //                localizedPointOfInterest = new LocalizedPointOfInterest()
        //                {
        //                    Id = pointOfInterestId,
        //                    Name = parentRegion.ParentRegionName,
        //                    CreatorId = creatorId,
        //                    LanguageId = defaultLanguageId,
        //                    DateAndTimeOfCreation = DateTime.Now
        //                };

        //                if (parentRegion.ParentRegionName.Trim() != parentRegion.ParentRegionNameLong.Trim())
        //                {
        //                    localizedPointOfInterest.LongName = parentRegion.ParentRegionNameLong;
        //                }

        //                localizedPointsOfInterest.Add(pointOfInterestId, localizedPointOfInterest);
        //            }
        //        }

        //        if (!mappingPointsOfInterestEanRegionIdsToIds.TryGetValue(parentRegion.RegionID, out pointOfInterestId)) continue;
        //        if (localizedPointsOfInterest.ContainsKey(pointOfInterestId)) continue;
        //        localizedPointOfInterest = new LocalizedPointOfInterest()
        //        {
        //            Id = pointOfInterestId,
        //            Name = parentRegion.RegionName,
        //            CreatorId = creatorId,
        //            LanguageId = defaultLanguageId
        //        };

        //        if (parentRegion.RegionName.Trim() != parentRegion.RegionNameLong.Trim())
        //        {
        //            localizedPointOfInterest.LongName = parentRegion.RegionNameLong;
        //        }

        //        localizedPointsOfInterest.Add(pointOfInterestId, localizedPointOfInterest);
        //    }

        //    return localizedPointsOfInterest.Values.ToArray();
        //}

        private static T[] BuildLocalizedRegions<T>(IEnumerable<ParentRegion> parentRegions,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds, int creatorId, int defaultLanguageId) where T : BaseLocalizedRegion, new()
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
                localizedRegion = new T
                {
                    Id = regionId,
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

        private static SubClass[] BuildSubClasses(IEnumerable<ParentRegion> entities, ICollection<string> subClasses, int creatorId)
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
