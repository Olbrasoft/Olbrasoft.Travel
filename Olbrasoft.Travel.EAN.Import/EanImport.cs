using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using Castle.DynamicProxy;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Olbrasoft.EntityFramework.Bulk;
#pragma warning disable 618
using static Castle.MicroKernel.Registration.AllTypes;
#pragma warning restore 618
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DAL.EntityFramework;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;
using Olbrasoft.Travel.EAN.DTO.Property;
using Olbrasoft.Travel.EAN.Import.Development;

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

            container.AddFacility<TypedFactoryFacility>();
            container.Register(Component.For<IFactoryOfRepositories>().AsFactory());

            var usersRepository = container.Resolve<IFactoryOfRepositories>().Users();
            user = usersRepository.AddIfNotExist(user);

            Write($"Id to a user with a UserName {user.UserName} is {user.Id}.");


            // var url = "https://www.ian.com/affiliatecenter/include/V2/ParentRegionList.zip";

            //// DownloadFile(url, runningStatus, importsFacade, import);

            //// todo Extract

            container.Register(Component.For<IImportProvider>().ImplementedBy<FileImportProvider>());
            container.Register(Component.For<User>().Instance(user));

            var languagesRepository = container.Resolve<ILanguagesRepository>();

            var defaultLanguage = languagesRepository.Get(1033);
            if (defaultLanguage == null)
            {
                defaultLanguage = new Language()
                {
                    Id = 1033,
                    EanLanguageCode = "en_US",
                    CreatorId = user.Id
                };
                languagesRepository.Add(defaultLanguage);
            }


            container.Register(
                Component.For<ImportOption>()
                .ImplementedBy<ImportOption>()
                .DependsOn(Dependency.OnValue("creatorId", user.Id), Dependency.OnValue("defaultLanguageId", defaultLanguage.Id))
            );

            
            container.Register(Component.For(typeof(IImport<ParentRegion>))
                .ImplementedBy(typeof(ParentRegionBatchImporter))
                .Interceptors<IInterceptor>()
            );


            container.Register(Component.For(typeof(IImport<DTO.Geography.Country>))
                .ImplementedBy<CountriesBatchImporter>()
                .Interceptors<IInterceptor>()
            );


            container.Register(Component.For(typeof(IImport<CityCoordinates>))
                .ImplementedBy<CitiesBatchImporter>()
                .Interceptors<IInterceptor>()
            );

            container.Register(Component.For(typeof(IImport<DTO.Geography.NeighborhoodCoordinates>))
                .ImplementedBy<NeighborhoodsBatchImporter>()
                .Interceptors<IInterceptor>()
            );

            container.Register(Component.For(typeof(IImport<PointOfInterestCoordinates>))
                .ImplementedBy<PointsOfInterestCoordinatesBatchImporter>()
                .Interceptors<IInterceptor>()
            );

            container.Register(Component.For(typeof(IImport<DTO.Geography.AirportCoordinates>))
                .ImplementedBy<AirportsBatchImporter>()
                .Interceptors<IInterceptor>()
            );

            container.Register(Component.For(typeof(IImport<TrainMetroStationCoordinates>))
                .ImplementedBy<TrainMetroStationsBatchImporter>()
                .Interceptors<IInterceptor>()
            );

            container.Register(Component.For(typeof(IImport<RegionCenter>))
                .ImplementedBy<RegionCenterBatchImporter>()
                .Interceptors<IInterceptor>()
            );

            container.Register(Component.For(typeof(IImport<PropertyType>))
                .ImplementedBy<TypesOfAccommodationsBatchImporter>()
                .Interceptors<IInterceptor>()
            );

            container.Register(Component.For(typeof(IImport<Travel.EAN.DTO.Property.Chain>))
                .ImplementedBy<ChainsBatchImporter>()
                .Interceptors<IInterceptor>()
            );

            container.Register(Component.For(typeof(IImport<ActiveProperty>))
                .ImplementedBy<AccommodationsBatchImporter>()
                .Interceptors<IInterceptor>()
            );

            container.Register(Component.For(typeof(IImport<Travel.EAN.DTO.Property.Description>))
                .ImplementedBy<DescriptionsBatchImporter>()
                .Interceptors<IInterceptor>()
            );


            container.Register(Component.For(typeof(IImport<Travel.EAN.DTO.Property.HotelImage>))
                .ImplementedBy<ImagesOfHotelsImporter>()
                .Interceptors<IInterceptor>()
            );

            container.Register(Component.For(typeof(IImport<Travel.EAN.DTO.Property.RoomType>))
                .ImplementedBy<RoomsTypesImporter>()
                .Interceptors<IInterceptor>()
            );

            
            //var parentRegionImporter = container.Resolve<IImport<ParentRegion>>();
            //parentRegionImporter.Import(@"D:\Ean\ParentRegionList.txt");
            
            //var countriesImporter = container.Resolve<IImport<DTO.Geography.Country>>();
            //countriesImporter.Import(@"D:\Ean\CountryList.txt");
            
            //var neighborhoodsImporter = container.Resolve<IImport<NeighborhoodCoordinates>>();
            //neighborhoodsImporter.Import(@"D:\Ean\NeighborhoodCoordinatesList.Txt");

            //var citiesImporter = container.Resolve<IImport<CityCoordinates>>();
            //citiesImporter.Import(@"D:\Ean\CityCoordinatesList.Txt");
            
            //var pointsOfInterestImporter = container.Resolve<IImport<PointOfInterestCoordinates>>();
            //pointsOfInterestImporter.Import(@"D:\Ean\PointsOfInterestCoordinatesList.txt");
            
            //var airportsImporter = container.Resolve<IImport<AirportCoordinates>>();
            //airportsImporter.Import(@"D:\Ean\AirportCoordinatesList.txt");
            
            //var trainMetroStationsImporter = container.Resolve<IImport<TrainMetroStationCoordinates>>();
            //trainMetroStationsImporter.Import(@"D:\Ean\TrainMetroStationCoordinatesList.txt");
            
            //var regionCenterImporter = container.Resolve<IImport<RegionCenter>>();
            //regionCenterImporter.Import(@"D:\Ean\RegionCenterCoordinatesList.txt");
            
            //var typesOfAccommodationsImporter = container.Resolve<IImport<PropertyType>>();
            //typesOfAccommodationsImporter.Import(@"D:\Ean\PropertyTypeList.txt");
            
            //var chainsImporter = container.Resolve<IImport<DTO.Property.Chain>>();
            //chainsImporter.Import(@"D:\Ean\ChainList.txt");




            //var accommodationsImporter = container.Resolve<IImport<ActiveProperty>>();
            //accommodationsImporter.Import(@"D:\Ean\ActivePropertyList.txt");

            //var descriptionsImporter = container.Resolve<IImport<Travel.EAN.DTO.Property.Description>>();
            //descriptionsImporter.Import(@"D:\Ean\PropertyDescriptionList.txt");


            container.Register(
                   Component.For<SharedProperties>()
                       .ImplementedBy<SharedProperties>()
                       .DependsOn(Dependency.OnValue("creatorId", user.Id), Dependency.OnValue("defaultLanguageId", defaultLanguage.Id))
               );


            container.Register(Component.For(typeof(IProvider)).ImplementedBy<FileImportProvider>().Named(nameof(FileImportProvider)));


           


            container.Register(Component.For(typeof(IImporter))
                   .ImplementedBy<PathsExtensionsCaptionsImporter>().Named(nameof(PathsExtensionsCaptionsImporter))
                   .Interceptors<IInterceptor>()
               );


            container.Register(Component.For(typeof(IImporter))
                .ImplementedBy<PhotosOfAccommodationsImporter>().Named(nameof(PhotosOfAccommodationsImporter))
                .Interceptors<IInterceptor>()
            );


            container.Register(Component.For(typeof(IImporter))
                .ImplementedBy<TypesOfRoomsImporter>().Named(nameof(TypesOfRoomsImporter))
                .Interceptors<IInterceptor>()
            );


            container.Register(Component.For(typeof(IImporter))
                .ImplementedBy<LocalizedTypesOfRoomsImporter>().Named(nameof(LocalizedTypesOfRoomsImporter))
                .Interceptors<IInterceptor>()
            );

            container.Register(Component.For(typeof(IImporter))
                .ImplementedBy<RoomsTypesImagesImporter>().Named(nameof(RoomsTypesImagesImporter))
                .Interceptors<IInterceptor>()
            );


            container.Register(Component.For(typeof(IImporter))
                .ImplementedBy<PhotosOfAccommodationsToTypesOfRoomsImporter>().Named(nameof(PhotosOfAccommodationsToTypesOfRoomsImporter))
                .Interceptors<IInterceptor>()
            );


            container.Register(Component.For(typeof(IImporter))
                .ImplementedBy<AttributesImporter>().Named(nameof(AttributesImporter))
                .Interceptors<IInterceptor>()
            );

            container.Register(Component.For(typeof(IImporter))
                .ImplementedBy<LocalizedAttributesDefaultLanguageImporter>().Named(nameof(LocalizedAttributesDefaultLanguageImporter))
                .Interceptors<IInterceptor>()
            );
            
            container.Register(Component.For(typeof(IImporter))
                .ImplementedBy<AccommodationsToAttributesDefaultLanguageImporter>().Named(nameof(AccommodationsToAttributesDefaultLanguageImporter))
                .Interceptors<IInterceptor>()
            );

           
           container.Register(Component.For(typeof(IImporter))
                .ImplementedBy<RegionsImporter>()
                .Named(nameof(RegionsImporter))
                .Interceptors<IInterceptor>()
            );

            //using (var subClassesImporter = container.Resolve<IImporter>(nameof(RegionsImporter)))
            //{
            //    subClassesImporter.Import(@"D:\Ean\ParentRegionList.txt");
            //}


            container.Register(Component.For(typeof(IImporter))
                .ImplementedBy<CountriesImporter>()
                .Named(nameof(CountriesImporter))
                .Interceptors<IInterceptor>()
            );

            //using (var countriesImporter = container.Resolve<IImporter>(nameof(CountriesImporter)))
            //{
            //    countriesImporter.Import(@"D:\Ean\CountryList.txt");
            //}


            container.Register(Component.For(typeof(IImporter))
                .ImplementedBy<NeighborhoodsImporter>()
                .Named(nameof(NeighborhoodsImporter))
                .Interceptors<IInterceptor>()
            );


            //using (var neighborhoodsImporter = container.Resolve<IImporter>(nameof(NeighborhoodsImporter)))
            //{
            //    neighborhoodsImporter.Import(@"D:\Ean\NeighborhoodCoordinatesList.txt");
            //}


            container.Register(Component.For(typeof(IImporter))
                .ImplementedBy<RegionsTypesOfCitiesImporter>()
                .Named(nameof(RegionsTypesOfCitiesImporter))
                .Interceptors<IInterceptor>()
            );

            //using (var citiesImporter = container.Resolve<IImporter>(nameof(RegionsTypesOfCitiesImporter)))
            //{
            //    citiesImporter.Import(@"D:\Ean\CityCoordinatesList.txt");
            //}


            container.Register(Component.For(typeof(IImporter))
                .ImplementedBy<PointsOfInterestImporter>()
                .Named(nameof(PointsOfInterestImporter))
                .Interceptors<IInterceptor>()
            );


            //using (var pointsOfInterestImporter = container.Resolve<IImporter>(nameof(PointsOfInterestImporter)))
            //{
            //    pointsOfInterestImporter.Import(@"D:\Ean\PointsOfInterestCoordinatesList.txt");
            //}


            container.Register(Component.For(typeof(IImporter))
                .ImplementedBy<AirportsImporter>()
                .Named(nameof(AirportsImporter))
                .Interceptors<IInterceptor>()
            );


            //using (var airportsImporter = container.Resolve<IImporter>(nameof(AirportsImporter)))
            //{
            //    airportsImporter.Import(@"D:\Ean\AirportCoordinatesList.txt");
            //}


            container.Register(Component.For(typeof(IImporter))
                .ImplementedBy<TrainMetroStationsImporter>()
                .Named(nameof(TrainMetroStationsImporter))
                .Interceptors<IInterceptor>()
            );


            //using (var trainMetroStationsImporter = container.Resolve<IImporter>(nameof(TrainMetroStationsImporter)))
            //{
            //    trainMetroStationsImporter.Import(@"D:\Ean\TrainMetroStationCoordinatesList.txt");
            //}


            container.Register(Component.For(typeof(IImporter))
                .ImplementedBy<RegionsCenterImporter>()
                .Named(nameof(RegionsCenterImporter))
                .Interceptors<IInterceptor>()
            );


            //using (var regionsCenterImporter = container.Resolve<IImporter>(nameof(RegionsCenterImporter)))
            //{
            //    regionsCenterImporter.Import(@"D:\Ean\RegionCenterCoordinatesList.txt");
            //}

            container.Register(Component.For(typeof(IImporter))
                .ImplementedBy<TypesOfAccommodationsImporter>()
                .Named(nameof(TypesOfAccommodationsImporter))
                .Interceptors<IInterceptor>()
            );

            //using (var typesOfAccommodationsImporter = container.Resolve<IImporter>(nameof(TypesOfAccommodationsImporter)))
            //{
            //    typesOfAccommodationsImporter.Import(@"D:\Ean\PropertyTypeList.txt");

            //}


            container.Register(Component.For(typeof(IImporter))
                .ImplementedBy<ChainsImporter>()
                .Named(nameof(ChainsImporter))
                .Interceptors<IInterceptor>()
            );

            //using (var chainsImporter= container.Resolve<IImporter>(nameof(ChainsImporter)))
            //{
            //    chainsImporter.Import(@"D:\Ean\ChainList.txt");
            //}







            //using (var pathsExtensionsCaptionsImporter =
            //    container.Resolve<IImporter>(nameof(PathsExtensionsCaptionsImporter)))
            //{
            //    pathsExtensionsCaptionsImporter.Import(@"D:\Ean\HotelImageList.txt");
            //}

            //using (var photosOfAccommodationsImporter =
            //    container.Resolve<IImporter>(nameof(PhotosOfAccommodationsImporter)))
            //{
            //    photosOfAccommodationsImporter.Import(@"D:\Ean\HotelImageList.txt");
            //}

            //using (var typesOfRoomsImporter = container.Resolve<IImporter>(nameof(TypesOfRoomsImporter)))
            //{
            //    typesOfRoomsImporter.Import(@"D:\Ean\RoomTypeList.txt");
            //}

            //using (var localizedTypesOfRoomsImporter =
            //    container.Resolve<IImporter>(nameof(LocalizedTypesOfRoomsImporter)))
            //{
            //    localizedTypesOfRoomsImporter.Import(@"D:\Ean\RoomTypeList.txt");
            //}

            //using (var roomsTypesImagesImporter = container.Resolve<IImporter>(nameof(RoomsTypesImagesImporter)))
            //{
            //    roomsTypesImagesImporter.Import(@"D:\Ean\RoomTypeList.txt");
            //}

            //using (var photosOfAccommodationsToTypesOfRoomsImporter = container.Resolve<IImporter>(nameof(PhotosOfAccommodationsToTypesOfRoomsImporter)))
            //{
            //    photosOfAccommodationsToTypesOfRoomsImporter.Import(@"D:\Ean\RoomTypeList.txt");
            //}

            //using (var attributesImporter = container.Resolve<IImporter>(nameof(AttributesImporter)))
            //{
            //    attributesImporter.Import(@"D:\Ean\AttributeList.txt");
            //}

            //using (var attributesImporter = container.Resolve<IImporter>(nameof(LocalizedAttributesDefaultLanguageImporter)))
            //{
            //    attributesImporter.Import(@"D:\Ean\AttributeList.txt");
            //}

            //using (var accommodationsToAttributesDefaultLanguageImporter = container.Resolve<IImporter>(nameof(AccommodationsToAttributesDefaultLanguageImporter)))
            //{
            //    accommodationsToAttributesDefaultLanguageImporter.Import(@"D:\Ean\PropertyAttributeLink.txt");
            //}



            //var develepmentRoomsTypesImporter = new Development.DevelopmentRoomsTypesImporter(container.Resolve<ImportOption>());
            //develepmentRoomsTypesImporter.Import(@"D:\Ean\RoomTypeList.txt");

            //var roomsTypesImporter = container.Resolve<IImport<RoomType>>();
            //roomsTypesImporter.Import(@"D:\Ean\RoomTypeList.txt");


            //var tasks = new List<Development.DevelopmentTask>()
            //{
            //    new DevelopmentTask {Name = "Úkol1"},
            //    new DevelopmentTask() {Name = "ukol2"}
            //};


            //using (var ctx= new Development.DevelopmentContext()   )
            //{
            //    ctx.BulkDev(tasks,new BulkConfig(){KeepIdentity = true} );
            //}


            //Logger.Log(tasks[2].Id.ToString());


            Write("Imported");
#if DEBUG
            Console.ReadLine();
#endif
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
            container.Register(Component.For<DbContext>().ImplementedBy<TravelContext>().Named(nameof(TravelContext)));

            //container.Register(FromAssemblyNamed("Olbrasoft.Travel.BLL")
            //    .Where(type => type.Name.EndsWith("Facade"))
            //    .WithService.AllInterfaces()
            //);

            container.Register(FromAssemblyNamed("Olbrasoft.Travel.EAN")
                .Where(type => type.Name.EndsWith("Parser"))
                .WithService.AllInterfaces()
            );


            container.Register(FromAssemblyNamed("Olbrasoft.Travel.DAL.EntityFramework")
                   .Where(type => type.Name.EndsWith("Repository"))
                   .WithService.AllInterfaces()
               );

#if DEBUG
            container.Register(Component.For<ILoggingImports>().ImplementedBy<ConsoleLogger>());


#else
            container.Register(Component.For<ILoggingImports>().ImplementedBy<ConsoleLogger>());

              
#endif
            container.Register(Component.For<IParserFactory>().ImplementedBy<ParserFactory>());

            container.Register(Component.For(typeof(IAdditionalRegionsInfoRepository<>)).ImplementedBy(typeof(AdditionalRegionsInfoRepository<>)));

            container.Register(Component.For(typeof(ITypesRepository<>)).ImplementedBy(typeof(TypesRepository<>)));

            container.Register(Component.For(typeof(IManyToManyRepository<>)).ImplementedBy(typeof(ManyToManyRepository<>)));

            container.Register(Component.For(typeof(ILocalizedRepository<>)).ImplementedBy(typeof(LocalizedRepository<>)));

            container.Register(Component.For(typeof(IMappedEntitiesRepository<>)).ImplementedBy(typeof(MappedEntitiesRepository<>)));

            container.Register(Component.For<IInterceptor>().ImplementedBy<ImportInterceptor>());

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
