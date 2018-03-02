using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Olbrasoft.EntityFramework.Bulk;
#pragma warning disable 618
using static Castle.MicroKernel.Registration.AllTypes;
#pragma warning restore 618
using Olbrasoft.Travel.BLL;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DAL.EntityFramework;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;
using PointOfInterest = Olbrasoft.Travel.DTO.PointOfInterest;

namespace Olbrasoft.Travel.EAN.Import
{
    class EanImport
    {
        public static ILogger Logger = new ConsoleLogger();
        //public static int UserId;

        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            var user = new User
            {
                UserName = "EanImport"
            };

            var container = BuildContainer();
            WriteContent(container);

            var usersFacade = container.Resolve<IUsersFacade>();
            usersFacade.AddIfNotExist(ref user);

            Write($"Id to a user with a UserName {user.UserName} is {user.Id}.");


            #region Sucess

            // #region Comment


            // var url = "https://www.ian.com/affiliatecenter/include/V2/ParentRegionList.zip";

            //// DownloadFile(url, runningStatus, importsFacade, import);

            //// todo Extract

            var loogerImports = container.Resolve<ILoggingImports>();

            loogerImports.Log("ParentsRegions Load");
            var parentRegionListFullPath = @"D:\Ean\ParentRegionList.Txt";
            var parserFactory = container.Resolve<IParserFactory>();

            var parentRegions = new HashSet<ParentRegion>();
            using (var reader = new StreamReader(parentRegionListFullPath))
            {
                var parserParentRegion = parserFactory.Create<ParentRegion>(reader.ReadLine());

                while (!reader.EndOfStream)
                {
                    if (parserParentRegion.TryParse(reader.ReadLine(), out var parentRegion))
                    {
                        parentRegions.Add(parentRegion);
                    }
                }
            }
            loogerImports.Log("ParentRegions Loaded");

            Write("ParentsRegions is " + parentRegions.Count);

            var regionsFacade = container.Resolve<IRegionsFacade>();
            // var storedNamesOfTypesOfRegions = regionsFacade.GetNamesOfTypesOfRegions();

            // loogerImports.Log("SubClasses Build");
            // var subClassesOfRegionsToImport = new Dictionary<string, SubClass>();

            // foreach (var parentRegion in parentRegions)
            // {

            //     if (storedNamesOfTypesOfRegions.Contains(parentRegion.RegionType) &&
            //         !string.IsNullOrEmpty(parentRegion.SubClass) &&
            //         !subClassesOfRegionsToImport.ContainsKey(parentRegion.SubClass.ToLower()))
            //     {
            //         subClassesOfRegionsToImport.Add(
            //             parentRegion.SubClass,
            //             new SubClass { Name = parentRegion.SubClass.ToLower(), Creator = user }
            //             );
            //     }

            // }
            // loogerImports.Log("SubClasses Builded");

            // loogerImports.Log("SubClasses Saving.");
            // regionsFacade.Save(new HashSet<SubClass>(subClassesOfRegionsToImport.Values));
            // loogerImports.Log("SubClasses Saved.");

            // var storedTypesOfRegions = regionsFacade.TypesOfRegionsAsReverseDictionary(true);

            var mappingRegionIdsToIds = regionsFacade.GetMappingEanRegionIdsToIds();

            // loogerImports.Log("ParentRegions Build.");
            // var regions = new Dictionary<long, Region>();
            // foreach (var parentRegion in parentRegions)
            // {
            //     if (regions.ContainsKey(parentRegion.ParentRegionID)) continue;
            //     if (!storedTypesOfRegions.TryGetValue(parentRegion.ParentRegionType, out var typeOfRegionId)) continue;
            //     var region = new Region
            //     {
            //         EanRegionId = parentRegion.ParentRegionID,
            //         TypeOfRegionId = typeOfRegionId,
            //         CreatorId = user.Id,
            //         DateAndTimeOfCreation = DateTime.Now
            //     };

            //     if (mappingPointsOfInterestEanRegionIdsToIds.TryGetValue(parentRegion.ParentRegionID, out var id))
            //     {
            //         region.Id = id;
            //     }

            //     regions.Add(parentRegion.ParentRegionID, region);
            // }
            // loogerImports.Log("ParentRegions Builded.");

            var travelContext = container.Resolve<TravelContext>();

            // loogerImports.Log("ParentRegions Saving.");
            // travelContext.BulkInsert(regions.Values.Where(r => r.Id == 0));
            // travelContext.BulkUpdate(regions.Values.Where(r => r.Id != 0));
            // loogerImports.Log("ParentRegions Saved.");

            // var storedSubClasses = regionsFacade.SubClassesAsReverseDictionary(true);
            // mappingPointsOfInterestEanRegionIdsToIds = regionsFacade.GetMappingEanRegionIdsToIds(true);

            // loogerImports.Log("Regions Build.");
            // regions = new Dictionary<long, Region>();
            // foreach (var parentRegion in parentRegions)
            // {
            //     if (regions.ContainsKey(parentRegion.RegionID)) continue;
            //     if (!storedTypesOfRegions.TryGetValue(parentRegion.RegionType, out var typeOfRegionId)) continue;
            //     var region = new Region
            //     {
            //         EanRegionId = parentRegion.RegionID,
            //         TypeOfRegionId = typeOfRegionId,
            //         CreatorId = user.Id,
            //         DateAndTimeOfCreation = DateTime.Now
            //     };

            //     if (mappingPointsOfInterestEanRegionIdsToIds.TryGetValue(parentRegion.RegionID, out var id))
            //     {
            //         region.Id = id;
            //     }

            //     if (storedSubClasses.TryGetValue(parentRegion.SubClass, out var subClassId))
            //     {
            //         region.SubClassId = subClassId;
            //     }

            //     regions.Add(parentRegion.RegionID, region);
            // }
            // loogerImports.Log("Regions Builded.");

            // loogerImports.Log("Regions Saving.");
            // travelContext.BulkInsert(regions.Values.Where(r => r.Id == 0));
            // travelContext.BulkUpdate(regions.Values.Where(r => r.Id != 0));
            // loogerImports.Log("Regions Saved.");


            var mappingPointsOfInterestEanRegionIdsToIds =
                regionsFacade.GetMappingPointsOfInterestEanRegionIdsToIds();

            // loogerImports.Log("PointsOfInterests from ParentRegions Build.");
            // var pointsOfInterests = new Dictionary<long, PointOfInterest>();
            // foreach (var sourcePointOfInterest in parentRegions)
            // {
            //     if (!sourcePointOfInterest.ParentRegionType.StartsWith("Point of Interest")) continue;
            //     if (pointsOfInterests.ContainsKey(sourcePointOfInterest.ParentRegionID)) continue;

            //     var pointOfInterest = new PointOfInterest
            //     {
            //         EanRegionId = sourcePointOfInterest.ParentRegionID,
            //         CreatorId = user.Id,
            //         DateAndTimeOfCreation = DateTime.Now
            //     };

            //     if (mappingPointsOfInterestEanRegionIdsToIds.TryGetValue(sourcePointOfInterest.ParentRegionID, out var id))
            //     {
            //         pointOfInterest.Id = id;
            //     }

            //     pointOfInterest.Shadow = sourcePointOfInterest.ParentRegionType.EndsWith("Shadow");
            //     pointsOfInterests.Add(sourcePointOfInterest.ParentRegionID, pointOfInterest);
            // }
            // loogerImports.Log("PointsOfInterests from ParentRegions Builded.");

            // loogerImports.Log("PointsOfInterests from ParentRegions Saving.");
            // travelContext.BulkInsert(pointsOfInterests.Values.Where(r => r.Id == 0));
            // travelContext.BulkUpdate(pointsOfInterests.Values.Where(r => r.Id != 0));
            // loogerImports.Log("PointsOfInterests from ParentRegions Saved.");

            //mappingPointsOfInterestEanRegionIdsToIds =
            //    regionsFacade.GetMappingPointsOfInterestEanRegionIdsToIds(true);

            // loogerImports.Log("PointsOfInterests from Regions Build.");
            // pointsOfInterests = new Dictionary<long, PointOfInterest>();
            // foreach (var sourcePointOfInterest in parentRegions)
            // {
            //     if (!sourcePointOfInterest.RegionType.StartsWith("Point of Interest")) continue;
            //     if (pointsOfInterests.ContainsKey(sourcePointOfInterest.RegionID)) continue;

            //     var pointOfInterest = new PointOfInterest
            //     {
            //         EanRegionId = sourcePointOfInterest.RegionID,
            //         CreatorId = user.Id,
            //         DateAndTimeOfCreation = DateTime.Now
            //     };

            //     if (mappingPointsOfInterestEanRegionIdsToIds.TryGetValue(sourcePointOfInterest.RegionID, out var id))
            //     {
            //         pointOfInterest.Id = id;
            //     }

            //     if (storedSubClasses.TryGetValue(sourcePointOfInterest.SubClass, out var subClassId))
            //     {
            //         pointOfInterest.SubClassId = subClassId;
            //     }

            //     pointOfInterest.Shadow = sourcePointOfInterest.RegionType.EndsWith("Shadow");
            //     pointsOfInterests.Add(sourcePointOfInterest.RegionID, pointOfInterest);
            // }
            // loogerImports.Log("PointsOfInterests from Regions Builded.");

            // loogerImports.Log("PointsOfInterests from Regions Saving.");
            // travelContext.BulkInsert(pointsOfInterests.Values.Where(r => r.Id == 0));
            // travelContext.BulkUpdate(pointsOfInterests.Values.Where(r => r.Id != 0));
            // loogerImports.Log("PointsOfInterests from Regions Saved.");

            // #endregion
            #endregion

            var languagesFacade = container.Resolve<ILanguagesFacade>();
            var defaultLanguage = languagesFacade.Get(1033);
            if (defaultLanguage == null)
            {
                defaultLanguage = new Language()
                {
                    Id = 1033,
                    EanLanguageCode = "en_US",
                    Creator = user
                };
                languagesFacade.Add(defaultLanguage);
            }

            mappingRegionIdsToIds = regionsFacade.GetMappingEanRegionIdsToIds(true);

            loogerImports.Log("LocalizedRegions Build.");
            var localizedRegions = BuildLocalizedRegions(parentRegions, mappingRegionIdsToIds, user, defaultLanguage);
            loogerImports.Log("LocalizedRegions Builded.");

            var localizedFacade = container.Resolve<ILocalizedFacade>();

            loogerImports.Log("LocalizedRegions Saving.");
            if (!localizedFacade.Exists<LocalizedRegion>(defaultLanguage))
            { travelContext.BulkInsert(localizedRegions.Values); }
            else { travelContext.BulkUpdate(localizedRegions.Values); }
            loogerImports.Log("LocalizedRegions Saved.");

            mappingPointsOfInterestEanRegionIdsToIds =
                regionsFacade.GetMappingPointsOfInterestEanRegionIdsToIds(true);

            loogerImports.Log("LocalizedPointsOfInterest Build.");
            var localizedPointsOfInterest = BuildLocalizedPointsOfInterest(parentRegions,
                mappingPointsOfInterestEanRegionIdsToIds, user, defaultLanguage);
            loogerImports.Log("LocalizedPointsOfInterest Builded.");

            loogerImports.Log("LocalizedPointsOfInterest Save.");
            if (!localizedFacade.Exists<LocalizedPointOfInterest>(defaultLanguage))
            { travelContext.BulkInsert(localizedPointsOfInterest.Values); }
            else { travelContext.BulkUpdate(localizedPointsOfInterest.Values); }
            loogerImports.Log("LocalizedPointsOfInterest Saved.");

            loogerImports.Log("RegionsToRegions Build.");
            var storedRegionsToRegions = travelContext.RegionsToRegions.ToDictionary(k => k.RegionId, v => v.ParentRegionId);
            var regionsToRegions = BuildRegionsToRegions(parentRegions, mappingRegionIdsToIds, storedRegionsToRegions);
            loogerImports.Log("RegionsToRegions Builded.");

            loogerImports.Log("RegionsToRegions Save.");
            var regionsToRegionsArray = regionsToRegions as RegionToRegion[] ?? regionsToRegions.ToArray();
            if (regionsToRegionsArray.Length != 0) travelContext.BulkInsert(regionsToRegionsArray);
            loogerImports.Log("RegionsToRegions Saved.");
            
            loogerImports.Log("PointsOfInterestToPointsOfInterest Build.");
            var storedPointsOfInterestToPointsOfInterest = travelContext
                .PointsOfInterestToPointsOfInterest.ToDictionary(k => k.PointOfInterestId, v => v.ParentPointOfInterestId);
            var pointsOfInterestToPointsOfInterest = BuildPointsOfInterestToPointsOfInterest(parentRegions,
                mappingPointsOfInterestEanRegionIdsToIds, storedPointsOfInterestToPointsOfInterest);
            loogerImports.Log("PointsOfInterestToPointsOfInterest Builded.");

            loogerImports.Log("PointsOfInterestToPointsOfInterest Save.");
            var pointsOfInterestToPointsOfInterestArray =
                pointsOfInterestToPointsOfInterest as PointOfInterestToPointOfInterest[] ??
                pointsOfInterestToPointsOfInterest.ToArray();
            if(pointsOfInterestToPointsOfInterestArray.Length!=0) travelContext.BulkInsert(pointsOfInterestToPointsOfInterestArray);
            loogerImports.Log("PointsOfInterestToPointsOfInterest Saved.");

            loogerImports.Log("PointsOfInterestToRegions Build.");
            var storedPointsOfInterestToRegions = travelContext.PointsOfInterestToRegions
                .ToDictionary(k => k.PointOfInterestId, v => v.RegionId);
            var pointsOfInterestToRegions = BuildPointsOfInterestToRegions(parentRegions, mappingRegionIdsToIds,
                mappingPointsOfInterestEanRegionIdsToIds, storedPointsOfInterestToRegions);
            loogerImports.Log("PointsOfInterestToRegions Builded.");
            
            loogerImports.Log("PointsOfInterestToRegions Save.");
            var pointsOfInterestToRegionsArray = pointsOfInterestToRegions as PointOfInterestToRegion[] ??
                                                 pointsOfInterestToRegions.ToArray();
            if(pointsOfInterestToRegionsArray.Length!=0) travelContext.BulkInsert(pointsOfInterestToRegionsArray);
            loogerImports.Log("PointsOfInterestToRegions Saved.");

            Write("Imported");
#if DEBUG
            Console.ReadLine();
#endif
        }

        
        private static IEnumerable<PointOfInterestToRegion> BuildPointsOfInterestToRegions(
            IEnumerable<ParentRegion> parentRegions,
            IDictionary<long, int> mappingRegionIdsToIds, 
            IDictionary<long, int> mappingPointsOfInterestEanRegionIdsToIds, 
            IReadOnlyDictionary<int, int> storedPointsOfInterestToRegions)
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

            return pointsOfInterestToRegions;
        }


        private static IEnumerable<PointOfInterestToPointOfInterest> BuildPointsOfInterestToPointsOfInterest
          (
            IEnumerable<ParentRegion> parentRegions,
            IDictionary<long, int> mappingPointsOfInterestEanRegionIdsToIds,
            IReadOnlyDictionary<int, int> storedPointsOfInterestToPointsOfInterest
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

            return regionsToRegions;
        }


        private static IEnumerable<RegionToRegion> BuildRegionsToRegions(IEnumerable<ParentRegion> parentRegions,
            IDictionary<long, int> mappingRegionIdsToIds, IReadOnlyDictionary<int, int> storedRegionsToRegions)
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

            return regionsToRegions;
        }

        private static Dictionary<int, LocalizedPointOfInterest> BuildLocalizedPointsOfInterest(IEnumerable<ParentRegion> parentRegions, IDictionary<long, int> mappingPointsOfInterestEanRegionIdsToIds, User user,
            Language defaultLanguage)
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
                            CreatorId = user.Id,
                            LanguageId = defaultLanguage.Id,
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
                    CreatorId = user.Id,
                    LanguageId = defaultLanguage.Id,
                    DateAndTimeOfCreation = DateTime.Now
                };

                if (parentRegion.RegionName.Trim() != parentRegion.RegionNameLong.Trim())
                {
                    localizedPointOfInterest.LongName = parentRegion.RegionNameLong;
                }

                localizedPointsOfInterest.Add(pointOfInterestId, localizedPointOfInterest);
            }

            return localizedPointsOfInterest;
        }

        private static Dictionary<int, LocalizedRegion> BuildLocalizedRegions(IEnumerable<ParentRegion> parentRegions, IDictionary<long, int> mappingRegionIdsToIds, User user,
            Language defaultLanguage)
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
                            CreatorId = user.Id,
                            LanguageId = defaultLanguage.Id,
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
                    CreatorId = user.Id,
                    LanguageId = defaultLanguage.Id,
                    DateAndTimeOfCreation = DateTime.Now
                };

                if (parentRegion.RegionName.Trim() != parentRegion.RegionNameLong.Trim())
                {
                    localizedRegion.LongName = parentRegion.RegionNameLong;
                }

                localizedRegions.Add(regionId, localizedRegion);
            }

            return localizedRegions;
        }

        private static async void DownloadFile(string url)
        {

            var fileName = System.IO.Path.GetFileName(url);

            using (var wc = new WebClient())
            {
                await wc.DownloadFileTaskAsync(new Uri(url), @"D:\Ean\" + fileName);
            }
        }


        private static void WriteContent(IWindsorContainer container)
        {
#if DEBUG
            foreach (var handler in container.Kernel
                .GetAssignableHandlers(typeof(object)))
            {
                Write($"{handler.ComponentModel.Services} {handler.ComponentModel.Implementation}");
            }
#endif
        }


        private static WindsorContainer BuildContainer()
        {
            var container = new WindsorContainer();

            container.Register(Component.For<TravelContext>().ImplementedBy<TravelContext>());

            container.Register(FromAssemblyNamed("Olbrasoft.Travel.DAL.EntityFramework")
                .Where(type => type.Name.EndsWith("Repository"))
                .WithService.AllInterfaces()
            );

            container.Register(Component.For<ILoggingImports>().ImplementedBy<ImportsLogger>());

            container.Register(FromAssemblyNamed("Olbrasoft.Travel.BLL")
                .Where(type => type.Name.EndsWith("Facade"))
                .WithService.AllInterfaces()
            );

            container.Register(Component.For<IParserFactory>().ImplementedBy<ParserFactory>());

            container.Register(Component.For(typeof(ITravelRepository<>)).ImplementedBy(typeof(TravelRepository<>)));

            return container;
        }





        public static void Write(object s)
        {
#if DEBUG
            Logger.Log(s.ToString());
#endif
        }




    }
}
