using System;
using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.BLL;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;
using PointOfInterest = Olbrasoft.Travel.DTO.PointOfInterest;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class ParentRegionImporter : Importer<ParentRegion>
    {
        protected new readonly ParentRegionImportOption Option;
        protected ISubClassesFacade SubClassesFacade;
        protected IRegionsFacade RegionsFacade;
        protected IPointsOfInterestFacade PointsOfInterestFacade;

        public ParentRegionImporter(ParentRegionImportOption option) : base(option)
        {
            Option = option;
            SubClassesFacade = Option.SubClassesFacade;
            RegionsFacade = Option.RegionsFacade;
            PointsOfInterestFacade = Option.PointsOfInterestFacade;
        }

        protected override void ImportBatch(IEnumerable<ParentRegion> Cities)
        {
            var parentRegionsArray = Cities as ParentRegion[] ?? Cities.ToArray();

            ImportSubClasses(parentRegionsArray, SubClassesFacade, CreatorId);

            ImportRegions(parentRegionsArray, RegionsFacade, SubClassesFacade, CreatorId);

            ImportRegionsToRegions(parentRegionsArray, RegionsFacade);

            ImportPointsOfInterest(parentRegionsArray, PointsOfInterestFacade, SubClassesFacade, CreatorId);

            ImportPointsOfInterestToPointsOfInterest(parentRegionsArray, PointsOfInterestFacade);

            ImportPointsOfInterestToRegions(parentRegionsArray, RegionsFacade, PointsOfInterestFacade);

            ImportLocalizedRegions(parentRegionsArray, LocalizedFacade, RegionsFacade, CreatorId, DefaultLanguageId);

            ImportLocalizedPointsOfInterest(parentRegionsArray, LocalizedFacade, PointsOfInterestFacade, CreatorId, DefaultLanguageId);
        }

        private void ImportPointsOfInterestToRegions(IEnumerable<ParentRegion> parentRegions, IRegionsFacade regionsFacade, IPointsOfInterestFacade pointsOfInterestFacade)
        {
            WriteLog("PointsOfInterestToRegions Build.");
            var pointsOfInterestToRegions = BuildPointsOfInterestToRegions(parentRegions,
                regionsFacade.GetMappingEanRegionIdsToIds(), pointsOfInterestFacade.GetMappingEanRegionIdsToIds(),
                pointsOfInterestFacade.PointOfInterestIdsToRegionIds());
            var count = pointsOfInterestToRegions.Length;
            WriteLog($"PointsOfInterestToRegions Builded:{count}.");

            if (count <= 0) return;
            WriteLog("PointsOfInterestToPointsOfInterest Save.");
            pointsOfInterestFacade.BulkSave(pointsOfInterestToRegions);
            WriteLog("PointsOfInterestToPointsOfInterest Saved.");
        }

        private void ImportPointsOfInterestToPointsOfInterest(IEnumerable<ParentRegion> parentRegions, IPointsOfInterestFacade facade)
        {
            WriteLog("PointsOfInterestToPointsOfInterest Build.");
            var pointsOfInterestToPointsOfInterest = BuildPointsOfInterestToPointsOfInterest(parentRegions,
                facade.GetMappingEanRegionIdsToIds(true), facade.PointOfInterestIdsToParentPointOfInterestIds(true));
            var count = pointsOfInterestToPointsOfInterest.Length;
            WriteLog($"PointsOfInterestToPointsOfInterest Builded:{count}.");

            if (count <= 0) return;
            WriteLog("PointsOfInterestToPointsOfInterest Save.");
            facade.BulkSave(pointsOfInterestToPointsOfInterest);
            WriteLog("PointsOfInterestToPointsOfInterest Saved.");
        }

        private void ImportRegionsToRegions(IEnumerable<ParentRegion> parentRegions, IRegionsFacade regionsFacade)
        {
            WriteLog("RegionsToRegions Build.");
            var regionsToRegions = BuildRegionsToRegions(parentRegions, regionsFacade.GetMappingEanRegionIdsToIds(),
                regionsFacade.RegionIdsToParentRegionIds());
            var count = regionsToRegions.Length;
            WriteLog($"RegionsToRegions Builded:{count}.");

            if (count <= 0) return;
            WriteLog("RegionsToRegions Save.");
            regionsFacade.BulkSave(regionsToRegions);
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


        private void ImportLocalizedRegions(IEnumerable<ParentRegion> parentRegions, ILocalizedFacade facade, IRegionsFacade regionsFacade,
            int creatorId, int defaultLanguageId)
        {
            WriteLog("LocalizedRegions Build.");
            var localizedRegions = BuildLocalizedRegions(parentRegions, regionsFacade.GetMappingEanRegionIdsToIds(true),
                creatorId, defaultLanguageId);
            var count = localizedRegions.Length;
            WriteLog($"LocalizedRegions from Regions Builded:{count}.");

            if (count <= 0) return;
            WriteLog("LocalizedRegions Save.");
            facade.BulkSave(localizedRegions);
            WriteLog("LocalizedRegions Saved.");
        }
        
        private void ImportPointsOfInterest(ParentRegion[] parentRegions, IPointsOfInterestFacade facade,
            ISubClassesFacade subClassesFacade, int creatorId)
        {

            ImportParentPointsOfInterest(parentRegions, facade, creatorId);

            WriteLog("PointsOfInterests from Regions Build.");
            var pointsOfInterest = BuildPointsOfInterest(parentRegions, facade.GetMappingEanRegionIdsToIds(true),
                subClassesFacade.SubClassesAsReverseDictionary(true), creatorId);
            var count = pointsOfInterest.Length;
            WriteLog($"PointsOfInterests from Regions Builded:{count} new.");

            if (count <= 0) return;
            WriteLog("PointsOfInterest Save.");
            facade.BulkSave(pointsOfInterest);
            WriteLog("PointsOfInterest Saved.");
        }

        private void ImportParentPointsOfInterest(IEnumerable<ParentRegion> parentRegions, IPointsOfInterestFacade facade, int creatorId)
        {
            WriteLog("PointsOfInterest from ParentRegions Build.");
            var pointsOfInterest = BuildPointsOfInterestFromParentRegions
                (parentRegions, facade.GetMappingEanRegionIdsToIds(), creatorId);
            var count = pointsOfInterest.Length;
            WriteLog($"PointsOfInterest from ParentRegions Builded:{count}.");

            if (count <= 0) return;
            WriteLog("PointsOfInterest Save.");
            facade.BulkSave(pointsOfInterest);
            WriteLog("PointsOfInterest Saved.");
        }

        private static PointOfInterestToRegion[] BuildPointsOfInterestToRegions(
            IEnumerable<ParentRegion> parentRegions,
            IDictionary<long, int> mappingRegionIdsToIds,
            IDictionary<long, int> mappingPointsOfInterestEanRegionIdsToIds,
            IDictionary<int, int> storedPointsOfInterestToRegions)
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
                    RegionId = regionId,
                    PointOfInterestId = pointOfInterestId
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
            IDictionary<long, int> mappingPointsOfInterestEanRegionIdsToIds,
            IDictionary<int, int> storedPointsOfInterestToPointsOfInterest
        )
        {
            var regionsToRegions = new HashSet<PointOfInterestToPointOfInterest>();
            foreach (var parentRegion in parentRegions)
            {
                if (!parentRegion.ParentRegionType.StartsWith("Point of Interest")
                    ||
                    !parentRegion.RegionType.StartsWith("Point of Interest"))
                    continue;

                if (!mappingPointsOfInterestEanRegionIdsToIds.TryGetValue(parentRegion.ParentRegionID, out var parentPointOfInterestId)
                    ||
                    !mappingPointsOfInterestEanRegionIdsToIds.TryGetValue(parentRegion.RegionID, out var pointOfInterestId)
                ) continue;

                if (storedPointsOfInterestToPointsOfInterest.TryGetValue(pointOfInterestId, out var storedParentRegionId)
                    &&
                    storedParentRegionId == parentPointOfInterestId)
                    continue;

                var pointOfInterestToPointOfInterest = new PointOfInterestToPointOfInterest()
                {
                    PointOfInterestId = pointOfInterestId,
                    ParentPointOfInterestId = parentPointOfInterestId
                };

                if (!regionsToRegions.Contains(pointOfInterestToPointOfInterest))
                {
                    regionsToRegions.Add(pointOfInterestToPointOfInterest);
                }
            }

            return regionsToRegions.ToArray();
        }

        private void ImportRegions(ParentRegion[] parentRegions, IRegionsFacade facade, ISubClassesFacade subClassesFacade, int creatorId)
        {
            var storedTypesOfRegions = facade.TypesOfRegionsAsReverseDictionary(true);

            ImportParentRegions(parentRegions, facade, storedTypesOfRegions, creatorId);

            WriteLog("Regions Build.");
            var regions = BuildRegions
             (parentRegions, storedTypesOfRegions, facade.GetMappingEanRegionIdsToIds(true),
                subClassesFacade.SubClassesAsReverseDictionary(true), creatorId);

            var count = regions.Length;
            WriteLog($"Regions Builded:{count}.");

            if (count <= 0) return;
            WriteLog("Regions Save.");
            facade.BulkSave(regions);
            WriteLog("Regions Saved.");
        }


        private void ImportParentRegions(IEnumerable<ParentRegion> parentRegions, IRegionsFacade facade, IDictionary<string, int> storedTypesOfRegions, int creatorId)
        {
            WriteLog("ParentRegions Build.");
            var regions = BuildParentRegions(
                parentRegions, storedTypesOfRegions, facade.GetMappingEanRegionIdsToIds(), creatorId);

            var count = regions.Length;
            WriteLog($"ParentRegions Builded:{count} Regions.");

            if (count <= 0) return;
            WriteLog("ParentRegions Save.");
            facade.BulkSave(regions);
            WriteLog("ParenRegions Saved.");
        }


        private void ImportSubClasses(IEnumerable<ParentRegion> parentRegions, ISubClassesFacade facade, int creatorId)
        {
            WriteLog("SubClasses Build.");
            var subClasses = BuildSubClasses(parentRegions, creatorId);
            var count = subClasses.Length;
            WriteLog($"SubClasses Builded:{count}.");

            if (count <= 0) return;
            WriteLog("SubClasses Saving.");
            facade.Save(subClasses);
            WriteLog("SubClasses Saved.");
        }


        private static RegionToRegion[] BuildRegionsToRegions(IEnumerable<ParentRegion> parentRegions,
            IDictionary<long, int> mappingRegionIdsToIds, IDictionary<int, int> storedRegionsToRegions)
        {
            var regionsToRegions = new HashSet<RegionToRegion>();
            foreach (var parentRegion in parentRegions)
            {
                if (parentRegion.ParentRegionType.StartsWith("Point of Interest") || parentRegion.RegionType.StartsWith("Point of Interest"))
                    continue;

                if (!mappingRegionIdsToIds.TryGetValue(parentRegion.ParentRegionID, out var parentRegionId)
                    ||
                    !mappingRegionIdsToIds.TryGetValue(parentRegion.RegionID, out var regionId)
                ) continue;

                if (storedRegionsToRegions.TryGetValue(regionId, out var storedParentRegionId) && storedParentRegionId == parentRegionId)
                    continue;

                var regionToRegion = new RegionToRegion()
                {
                    RegionId = regionId,
                    ParentRegionId = parentRegionId
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
                    LanguageId = defaultLanguageId,
                    DateAndTimeOfCreation = DateTime.Now
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
            IDictionary<long, int> mappingRegionIdsToIds, int creatorId, int defaultLanguageId)
        {
            var localizedRegions = new Dictionary<int, LocalizedRegion>();
            foreach (var parentRegion in parentRegions)
            {
                LocalizedRegion localizedRegion;
                if (mappingRegionIdsToIds.TryGetValue(parentRegion.ParentRegionID, out var regionId))
                {
                    if (!localizedRegions.ContainsKey(regionId))
                    {
                        localizedRegion = new LocalizedRegion()
                        {
                            RegionId = regionId,
                            Name = parentRegion.ParentRegionName,
                            CreatorId = creatorId,
                            LanguageId = defaultLanguageId,
                            DateAndTimeOfCreation = DateTime.Now
                        };

                        if (parentRegion.ParentRegionName.Trim() != parentRegion.ParentRegionNameLong.Trim())
                        {
                            localizedRegion.LongName = parentRegion.ParentRegionNameLong;
                        }

                        localizedRegions.Add(regionId, localizedRegion);
                    }
                }

                if (!mappingRegionIdsToIds.TryGetValue(parentRegion.RegionID, out regionId)) continue;
                if (localizedRegions.ContainsKey(regionId)) continue;
                localizedRegion = new LocalizedRegion()
                {
                    RegionId = regionId,
                    Name = parentRegion.RegionName,
                    CreatorId = creatorId,
                    LanguageId = defaultLanguageId,
                    DateAndTimeOfCreation = DateTime.Now
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
            IDictionary<long, int> mappingEanRegionIdsToIds, IDictionary<string, int> storedSubClasses, int creatorId)
        {
            var pointsOfInterest = new Dictionary<long, PointOfInterest>();
            foreach (var sourcePointOfInterest in parentRegions)
            {
                if (!sourcePointOfInterest.RegionType.StartsWith("Point of Interest")) continue;
                if (pointsOfInterest.ContainsKey(sourcePointOfInterest.RegionID)) continue;

                var pointOfInterest = new PointOfInterest
                {
                    EanRegionId = sourcePointOfInterest.RegionID,
                    CreatorId = creatorId,
                    DateAndTimeOfCreation = DateTime.Now
                };

                if (mappingEanRegionIdsToIds.TryGetValue(sourcePointOfInterest.RegionID, out var id))
                {
                    pointOfInterest.Id = id;
                }

                if (storedSubClasses.TryGetValue(sourcePointOfInterest.SubClass, out var subClassId))
                {
                    pointOfInterest.SubClassId = subClassId;
                }

                pointOfInterest.Shadow = sourcePointOfInterest.RegionType.EndsWith("Shadow");
                pointsOfInterest.Add(sourcePointOfInterest.RegionID, pointOfInterest);
            }

            return pointsOfInterest.Values.ToArray();
        }

        private static PointOfInterest[] BuildPointsOfInterestFromParentRegions(IEnumerable<ParentRegion> parentRegions,
            IDictionary<long, int> mappingEanRegionIdsToIds, int creatorId)
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
                    DateAndTimeOfCreation = DateTime.Now
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

        private static Region[] BuildRegions(IEnumerable<ParentRegion> parentRegions, IDictionary<string, int> storedTypesOfRegions,
             IDictionary<long, int> mappingEanRegionIdsToIds, IDictionary<string, int> storedSubClasses, int creatorId)
        {
            var regions = new Dictionary<long, Region>();
            foreach (var parentRegion in parentRegions)
            {
                if (regions.ContainsKey(parentRegion.RegionID)) continue;
                if (!storedTypesOfRegions.TryGetValue(parentRegion.RegionType, out var typeOfRegionId)) continue;
                var region = new Region
                {
                    EanRegionId = parentRegion.RegionID,
                    TypeOfRegionId = typeOfRegionId,
                    CreatorId = creatorId,
                    DateAndTimeOfCreation = DateTime.Now
                };

                if (mappingEanRegionIdsToIds.TryGetValue(parentRegion.RegionID, out var id))
                {
                    region.Id = id;
                }

                if (storedSubClasses.TryGetValue(parentRegion.SubClass, out var subClassId))
                {
                    region.SubClassId = subClassId;
                }

                regions.Add(parentRegion.RegionID, region);
            }
            return regions.Values.ToArray();
        }

        private static Region[] BuildParentRegions(IEnumerable<ParentRegion> parentRegions, IDictionary<string, int> typesOfRegions,
            IDictionary<long, int> mappingEanRegionIdsToIds, int creatorId)
        {
            var regions = new Dictionary<long, Region>();
            foreach (var parentRegion in parentRegions)
            {
                if (regions.ContainsKey(parentRegion.ParentRegionID)) continue;
                if (!typesOfRegions.TryGetValue(parentRegion.ParentRegionType, out var typeOfRegionId)) continue;
                var region = new Region
                {
                    EanRegionId = parentRegion.ParentRegionID,
                    TypeOfRegionId = typeOfRegionId,
                    CreatorId = creatorId,
                    DateAndTimeOfCreation = DateTime.Now
                };

                if (mappingEanRegionIdsToIds.TryGetValue(parentRegion.ParentRegionID, out var id))
                {
                    region.Id = id;
                }

                regions.Add(parentRegion.ParentRegionID, region);
            }
            return regions.Values.ToArray();
        }

        private static SubClass[] BuildSubClasses(IEnumerable<ParentRegion> entities, int creatorId)
        {
            var subClassesOfRegionsToImport = new Dictionary<string, SubClass>();

            foreach (var parentRegion in entities)
            {
                if (!String.IsNullOrEmpty(parentRegion.SubClass)
                    &&
                    !subClassesOfRegionsToImport.ContainsKey(parentRegion.SubClass.ToLower()))
                {
                    subClassesOfRegionsToImport.Add(
                        parentRegion.SubClass,
                        new SubClass { Name = parentRegion.SubClass.ToLower(), CreatorId = creatorId }
                    );
                }

            }
            return subClassesOfRegionsToImport.Values.ToArray();
        }
    }


}
