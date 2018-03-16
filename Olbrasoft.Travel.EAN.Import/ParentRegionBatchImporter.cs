using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;


namespace Olbrasoft.Travel.EAN.Import
{
    internal class ParentRegionBatchImporter : BatchImporter<ParentRegion>
    {
        public ParentRegionBatchImporter(ImportOption option) : base(option)
        {

        }

        public override void ImportBatch(ParentRegion[] parentRegions)
        {
            var staEanIdsToIds = ImportStatesOrProvinces(parentRegions,
                FactoryOfRepositories.Geo<State>(),
                FactoryOfRepositories.Geo<Travel.DTO.Country>().EanIdsToIds, CreatorId);

            ImportLocalizedRegions(parentRegions, FactoryOfRepositories.Localized<LocalizedState>(), staEanIdsToIds, DefaultLanguageId, CreatorId);

            var citiesRepository = FactoryOfRepositories.Geo<City>();

            var citiesEanIdsToIds = ImportCities(parentRegions, citiesRepository, CreatorId);

            ImportLocalizedCities(parentRegions, FactoryOfRepositories.Localized<LocalizedCity>(), citiesEanIdsToIds, DefaultLanguageId, CreatorId);
            
            var subClasses = ImportSubClasses(parentRegions, FactoryOfRepositories.BaseNames<SubClass>(), CreatorId);

            var typesOfRegions = FactoryOfRepositories.BaseNames<TypeOfRegion>().NamesToIds;

            var regionsRepository = FactoryOfRepositories.Geo<Region>();

            var eanIdsToIds = ImportRegions(parentRegions, regionsRepository, subClasses, typesOfRegions, CreatorId);

            ImportLocalizedRegions(parentRegions, FactoryOfRepositories.Localized<LocalizedRegion>(), eanIdsToIds, CreatorId, DefaultLanguageId);

            ImportRegionsToRegions(parentRegions, FactoryOfRepositories.ManyToMany<RegionToRegion>(), eanIdsToIds, CreatorId);

            var pointsOfInterestRepository = FactoryOfRepositories.Geo<PointOfInterest>();

            var poiEanIdsToIds = ImportPointsOfInterest(parentRegions, pointsOfInterestRepository, subClasses, CreatorId);

            ImportLocalizedRegions(parentRegions, FactoryOfRepositories.Localized<LocalizedPointOfInterest>(), poiEanIdsToIds, CreatorId, DefaultLanguageId);
            

            //  ImportPointsOfInterestToPointsOfInterest(
            //      parentRegions, poiEanRegionIdsToIds, FactoryOfRepositories.ManyToMany<PointOfInterestToPointOfInterest>(), CreatorId);

            //  ImportPointsOfInterestToRegions(parentRegions, regEanRegionIdsToIds, poiEanRegionIdsToIds,
            //      FactoryOfRepositories.ManyToMany<PointOfInterestToRegion>(), CreatorId);

            //  Logger.Log($"Load EanAirportIdsToIds from Regions");
            //var  eanRegionIdsToIds = regionsRepository.EanRegionIdsToIds;
            //  Logger.Log($"Ids regions loaded count:{eanRegionIdsToIds.Count}");

            //BulkSaveLocalized(localizedPointsOfInterest, FactoryOfRepositories.Localized<LocalizedPointOfInterest>(), DefaultLanguageId, Logger);

        }

        private void ImportLocalizedCities(IEnumerable<ParentRegion> parentRegions,
            IBaseRepository<LocalizedCity, int, int> repository,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            int languageId,
            int creatorId
            )
        {
            LogBuild<LocalizedCity>();
            var localizedCities = BuildLocalizedRegions<LocalizedCity>(parentRegions, eanIdsToIds, creatorId, languageId);
            var count = localizedCities.Length;
            LogBuilded(count);

            if (count <= 0) return;

            LogSave<LocalizedCity>();
            repository.BulkSave(localizedCities, lc => lc.Name);
            LogSaved<LocalizedCity>();
        }

        private IReadOnlyDictionary<long, int> ImportCities(ParentRegion[] parentRegions,
            IGeoRepository<City> repository,
            int creatorId
            )
        {
            LogBuild<City>();
            var cities = BuildCities(parentRegions, creatorId);
            var count = cities.Length;
            LogBuilded(count);

            if (count <= 0) return repository.EanIdsToIds;

            LogSave<City>();
            repository.BulkSave(cities, c => c.Coordinates);
            LogSaved<City>();

            return repository.EanIdsToIds;
        }

        private static City[] BuildCities(IEnumerable<ParentRegion> parentRegions, int creatorId)
        {
            return parentRegions.Where(pr => pr.RegionType == "City")
                  .Select(pr => new City { EanId = pr.RegionID, CreatorId = creatorId }).ToArray();
        }



        private void ImportLocalizedRegions<T>(
            IEnumerable<ParentRegion> parentRegions,
            IBaseRepository<T, int, int> repository,
            IReadOnlyDictionary<long, int> eanIdsToIds,
            int languageId,
            int creatorId

        ) where T : LocalizedRegionWithNameAndLongName, new()
        {
            LogBuild<T>();
            var localizedEntities = BuildLocalizedRegions<T>(parentRegions, eanIdsToIds, creatorId, languageId);
            var count = localizedEntities.Length;
            LogBuilded(count);

            if (count <= 0) return;

            LogSave<T>();
            repository.BulkSave(localizedEntities);
            LogSaved<T>();
        }



        private IReadOnlyDictionary<long, int> ImportStatesOrProvinces(ParentRegion[] parentRegions,
            IGeoRepository<State> repository,
            IReadOnlyDictionary<long, int> countriesEanIdsToIds,
            int creatorId
            )
        {
            LogBuild<State>();
            var statesOrProvinces = BuildStates(parentRegions, countriesEanIdsToIds, creatorId);
            var count = statesOrProvinces.Length;
            LogBuilded(count);

            if (count == 0) return repository.EanIdsToIds;
            LogSave<State>();
            repository.BulkSave(statesOrProvinces);
            LogSaved<State>();

            return repository.EanIdsToIds;
        }

        private static State[] BuildStates(IEnumerable<ParentRegion> parentRegions, IReadOnlyDictionary<long, int> countriesEanIdsToIds, int creatorId)
        {

            var statesOrProvinces = new Queue<State>();
            foreach (var parentRegion in parentRegions)
            {
                if (parentRegion.RegionType != "Province (State)" || !countriesEanIdsToIds.TryGetValue(parentRegion.ParentRegionID, out var countryId)) continue;

                var stateOrProvince = new State
                {
                    EanId = parentRegion.RegionID,
                    CountryId = countryId,
                    CreatorId = creatorId
                };

                statesOrProvinces.Enqueue(stateOrProvince);
            }

            return statesOrProvinces.ToArray();
        }


        //private void ImportLocalizedPointsOfInterest(IEnumerable<ParentRegion> parentRegions,
        //    IBaseRepository<LocalizedPointOfInterest, int, int> repository,
        //    IReadOnlyDictionary<long, int> eanIdsToIds,
        //    int creatorId,
        //    int languageId
        //    )
        //{
        //    LogBuild<LocalizedPointOfInterest>();
        //    var localizedPointsOfInterest = BuildLocalizedRegions<LocalizedPointOfInterest>(parentRegions, eanIdsToIds, creatorId, languageId);
        //    LogBuilded(localizedPointsOfInterest.Length);

        //    LogSave<LocalizedPointOfInterest>();
        //    repository.BulkSave(localizedPointsOfInterest);
        //    LogSaved<LocalizedPointOfInterest>();
        //}


        //private void ImportLocalizedRegions(IEnumerable<ParentRegion> parentRegions,
        //    IBaseRepository<LocalizedRegion, int, int> repository,
        //    IReadOnlyDictionary<long, int> eanIdsToIds,
        //    int creatorId,
        //    int languageId
        //    )
        //{
        //    LogBuild<LocalizedRegion>();
        //    var localizedRegions =
        //        BuildLocalizedRegions<LocalizedRegion>(parentRegions, eanIdsToIds, creatorId, languageId);
        //    LogBuilded(localizedRegions.Length);

        //    LogSaved<LocalizedRegion>();
        //    repository.BulkSave(localizedRegions);
        //    LogSaved<LocalizedRegion>();
        //}


        private void ImportPointsOfInterestToRegions(
            IEnumerable<ParentRegion> parentRegions, IReadOnlyDictionary<long, int> regEanRegionIdsToIds,
            IReadOnlyDictionary<long, int> poiEanRegionIdsToIds, IManyToManyRepository<PointOfInterestToRegion> repository,
            int creatorId)
        {
            LogBuild<PointOfInterestToRegion>();

            var pointsOfInterestToRegions = BuildPointsOfInterestToRegions(
                parentRegions,
                regEanRegionIdsToIds, poiEanRegionIdsToIds,
                //repository.IdsToToIds,
                creatorId
                );

            var count = pointsOfInterestToRegions.Length;
            WriteLog($"PointsOfInterestToRegions Builded:{count}.");

            if (count <= 0) return;
            WriteLog("PointsOfInterestToPointsOfInterest Save.");
            repository.BulkSave(pointsOfInterestToRegions);
            WriteLog("PointsOfInterestToPointsOfInterest Saved.");
        }

        private void ImportPointsOfInterestToPointsOfInterest(IEnumerable<ParentRegion> parentRegions,
            IReadOnlyDictionary<long, int> poiEanRegionIdsToIds,
            IManyToManyRepository<PointOfInterestToPointOfInterest> repository, int creatorId)
        {
            WriteLog("PointsOfInterestToPointsOfInterest Build.");
            var pointsOfInterestToPointsOfInterest = BuildPointsOfInterestToPointsOfInterest(
                parentRegions,
                poiEanRegionIdsToIds,
                //repository.IdsToToIds, 
                creatorId
                );

            var count = pointsOfInterestToPointsOfInterest.Length;
            WriteLog($"PointsOfInterestToPointsOfInterest Builded:{count}.");

            if (count <= 0) return;
            WriteLog("PointsOfInterestToPointsOfInterest Save.");
            repository.BulkSave(pointsOfInterestToPointsOfInterest);
            WriteLog("PointsOfInterestToPointsOfInterest Saved.");
        }

        private void ImportRegionsToRegions(IEnumerable<ParentRegion> parentRegions,
            IBaseRepository<RegionToRegion, int, int> repository,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds,
            int creatorId)
        {

            LogBuild<RegionToRegion>();
            var regionsToRegions = BuildRegionsToRegions(
                parentRegions,
                eanRegionIdsToIds,
                //repository.IdsToToIds,
                creatorId
                );

            var count = regionsToRegions.Length;

            LogBuilded(count);

            if (count <= 0) return;
            LogSave<RegionToRegion>();
            repository.BulkSave(regionsToRegions);
            LogSaved<RegionToRegion>();
        }


        private IReadOnlyDictionary<long, int> ImportPointsOfInterest(ParentRegion[] parentRegions,
            IMapToPartnersRepository<PointOfInterest, long> repository,
            IReadOnlyDictionary<string, int> subClasses, int creatorId)
        {
            ImportParentPointsOfInterest(parentRegions, repository, creatorId);

            LogBuild<PointOfInterest>();

            var pointsOfInterest = BuildPointsOfInterest(parentRegions,
                 subClasses, creatorId);

            var count = pointsOfInterest.Length;
            LogBuilded(count);

            if (count <= 0) return repository.EanIdsToIds;

            LogSave<PointOfInterest>();
            repository.BulkSave(pointsOfInterest);
            LogSaved<PointOfInterest>();

            return repository.EanIdsToIds;
        }

        private void ImportParentPointsOfInterest(IEnumerable<ParentRegion> parentRegions,
            IMapToPartnersRepository<PointOfInterest, long> repository,
            int creatorId)
        {
            WriteLog("PointsOfInterest from ParentRegions Build.");

            var pointsOfInterest = BuildPointsOfInterestFromParentRegions
                (parentRegions, creatorId);

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
            //IReadOnlyDictionary<int, int> storedPointsOfInterestToRegions,
            int creatorId
            )
        {
            var pointsOfInterestToRegions = new HashSet<PointOfInterestToRegion>();
            foreach (var parentRegion in parentRegions)
            {

                if (!mappingRegionIdsToIds.TryGetValue(parentRegion.ParentRegionID, out var regionId)
                    ||
                    !mappingPointsOfInterestEanRegionIdsToIds.TryGetValue(parentRegion.RegionID, out var pointOfInterestId)
                ) continue;

                //if (storedPointsOfInterestToRegions.TryGetValue(pointOfInterestId, out var storedParentRegionId) && storedParentRegionId == regionId)
                //    continue;

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
            // IReadOnlyDictionary<int, int> storedPointsOfInterestToPointsOfInterest,
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

                //if (storedPointsOfInterestToPointsOfInterest.TryGetValue(pointOfInterestId, out var storedParentRegionId)
                //    &&
                //    storedParentRegionId == parentPointOfInterestId)
                //    continue;

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

        private IReadOnlyDictionary<long, int> ImportRegions(ParentRegion[] parentRegions,
            IMapToPartnersRepository<Region, long> repository,
            IReadOnlyDictionary<string, int> subClasses,
            IReadOnlyDictionary<string, int> typesOfRegions,
            int creatorId)
        {
            ImportParentRegions(parentRegions, repository, typesOfRegions, creatorId);

            LogBuild<Region>();
            var regions = BuildRegions(parentRegions, typesOfRegions, subClasses, creatorId);
            var count = regions.Length;
            LogBuilded(count);

            if (count <= 0) return repository.EanIdsToIds;

            LogSave<Region>();
            repository.BulkSave(regions);
            LogSaved<Region>();

            return repository.EanIdsToIds;

        }


        private void ImportParentRegions(IEnumerable<ParentRegion> parentRegions,
            IMapToPartnersRepository<Region, long> repository,
            IReadOnlyDictionary<string, int> storedTypesOfRegions,
            int creatorId
            )
        {
            WriteLog("ParentRegions Build.");
            var regions = BuildParentRegions(
                parentRegions, storedTypesOfRegions, creatorId);

            var count = regions.Length;
            WriteLog($"ParentRegions Builded:{count} Regions.");

            if (count <= 0) return;
            WriteLog("ParentRegions Save.");
            repository.BulkSave(regions);
            WriteLog("ParenRegions Saved.");
        }


        private IReadOnlyDictionary<string, int> ImportSubClasses(IEnumerable<ParentRegion> parentRegions, IBaseNamesRepository<SubClass> repository, int creatorId)
        {

            LogBuild<SubClass>();
            var storedSubClasses = new HashSet<string>(repository.Names);
            var subClasses = BuildSubClasses(parentRegions, storedSubClasses, creatorId);
            var count = subClasses.Length;
            LogBuilded(count);

            if (count <= 0) return repository.NamesToIds;
            LogSave<SubClass>();
            repository.BulkSave(subClasses);
            LogSaved<SubClass>();

            return repository.NamesToIds;
        }


        private static RegionToRegion[] BuildRegionsToRegions(IEnumerable<ParentRegion> parentRegions,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds,
            //IReadOnlyDictionary<int, int> storedRegionsToRegions,
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

                //if (storedRegionsToRegions.TryGetValue(regionId, out var storedParentRegionId) && storedParentRegionId == parentRegionId)
                //    continue;

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
            IReadOnlyDictionary<long, int> eanRegionIdsToIds, int creatorId, int languageId) where T : LocalizedRegionWithNameAndLongName, new()
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


        private static PointOfInterest[] BuildPointsOfInterest(IEnumerable<ParentRegion> parentRegions,
             IReadOnlyDictionary<string, int> subClasses,
             int creatorId
            )
        {
            var pointsOfInterest = new Dictionary<long, PointOfInterest>();
            foreach (var sourcePointOfInterest in parentRegions)
            {
                if (!sourcePointOfInterest.RegionType.StartsWith("Point of Interest")) continue;
                if (pointsOfInterest.ContainsKey(sourcePointOfInterest.RegionID)) continue;

                var pointOfInterest = new PointOfInterest
                {
                    EanId = sourcePointOfInterest.RegionID,
                    CreatorId = creatorId,
                    Shadow = sourcePointOfInterest.RegionType.EndsWith("Shadow")
                };

                //if (subClasses.TryGetValue(sourcePointOfInterest.SubClass, out var subClassId))
                //{
                //    pointOfInterest.SubClassId = subClassId;
                //}

                pointsOfInterest.Add(sourcePointOfInterest.RegionID, pointOfInterest);
            }

            return pointsOfInterest.Values.ToArray();
        }

        private static PointOfInterest[] BuildPointsOfInterestFromParentRegions(IEnumerable<ParentRegion> parentRegions,
           int creatorId)
        {

            var pointsOfInterest = new Dictionary<long, PointOfInterest>();
            foreach (var sourcePointOfInterest in parentRegions)
            {
                if (!sourcePointOfInterest.ParentRegionType.StartsWith("Point of Interest")) continue;
                if (pointsOfInterest.ContainsKey(sourcePointOfInterest.ParentRegionID)) continue;

                var pointOfInterest = new PointOfInterest
                {
                    EanId = sourcePointOfInterest.ParentRegionID,
                    CreatorId = creatorId,
                    Shadow = sourcePointOfInterest.ParentRegionType.EndsWith("Shadow"),
                };

                //if (mappingEanRegionIdsToIds.TryGetValue(sourcePointOfInterest.ParentRegionID, out var id))
                //{
                //    pointOfInterest.Id = id;
                //}

                pointsOfInterest.Add(sourcePointOfInterest.ParentRegionID, pointOfInterest);
            }
            return pointsOfInterest.Values.ToArray();
        }

        private static Region[] BuildRegions(IEnumerable<ParentRegion> parentRegions, IReadOnlyDictionary<string, int> typesOfRegions,
              IReadOnlyDictionary<string, int> storedSubClasses, int creatorId)
        {
            var regions = new Dictionary<long, Region>();
            foreach (var parentRegion in parentRegions)
            {
                if (regions.ContainsKey(parentRegion.RegionID)) continue;
                if (!typesOfRegions.TryGetValue(parentRegion.RegionType, out var typeOfRegionId))
                {
                    continue;
                }

                var region = new Region
                {
                    EanId = parentRegion.RegionID,
                    TypeOfRegionId = typeOfRegionId,
                    CreatorId = creatorId
                };

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

            //var regions = new Queue<Region>();
            //var eanRegionIds = new HashSet<long>();

            //Parallel.ForEach(parentRegions, parentRegion =>
            //{
            //    lock (_lockMe)
            //    {
            //        if (eanRegionIds.Contains(parentRegion.RegionID)) return;
            //        if (!typesOfRegions.TryGetValue(parentRegion.RegionType, out var typeOfRegionId)) return;

            //        var region = new Region
            //        {
            //            EanId = parentRegion.RegionID,
            //            TypeOfRegionId = typeOfRegionId,
            //            CreatorId = creatorId
            //        };

            //        if (!string.IsNullOrEmpty(parentRegion.SubClass))
            //        {
            //            if (storedSubClasses.TryGetValue(parentRegion.SubClass, out var subClassId))
            //            {
            //                region.SubClassId = subClassId;
            //            }
            //        }

            //        regions.Enqueue(region);
            //        eanRegionIds.Add(parentRegion.RegionID);
            //    }
            //});

            //return regions.ToArray();
        }

        private static Region[] BuildParentRegions(IEnumerable<ParentRegion> parentRegions,
             IReadOnlyDictionary<string, int> typesOfRegions,
             int creatorId
            )
        {
            var regions = new Dictionary<long, Region>();
            foreach (var parentRegion in parentRegions)
            {
                if (regions.ContainsKey(parentRegion.ParentRegionID)) continue;
                if (!typesOfRegions.TryGetValue(parentRegion.ParentRegionType, out var typeOfRegionId)) continue;
                var region = new Region
                {
                    EanId = parentRegion.ParentRegionID,
                    TypeOfRegionId = typeOfRegionId,
                    CreatorId = creatorId
                };

                regions.Add(parentRegion.ParentRegionID, region);
            }
            return regions.Values.ToArray();

            // var regions = new Queue<Region>();
            // var eanRegionIds = new HashSet<long>();

            //Parallel.ForEach(parentRegions, parentRegion =>
            //{

            //    lock (_lockMe)
            //    {
            //        if (eanRegionIds.Contains(parentRegion.ParentRegionID)) return;
            //        if (!typesOfRegions.TryGetValue(parentRegion.ParentRegionType, out var typeOfRegionId)) return;

            //        var region = new Region
            //        {
            //            EanId = parentRegion.ParentRegionID,
            //            TypeOfRegionId = typeOfRegionId,
            //            CreatorId = creatorId
            //        };

            //        regions.Enqueue(region);
            //        eanRegionIds.Add(parentRegion.ParentRegionID);
            //    }
            //});

            //return regions.ToArray();
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
