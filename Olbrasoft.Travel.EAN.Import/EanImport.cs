using System;
using System.Data.Entity;
using System.Net;
using Castle.DynamicProxy;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
#pragma warning disable 618
using static Castle.MicroKernel.Registration.AllTypes;
#pragma warning restore 618
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DAL.EntityFramework;
using Olbrasoft.Travel.DTO;

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
            
            var usersRepository = container.Resolve<IFactoryOfRepositories>().Users();
            user = usersRepository.AddIfNotExist(user);

            Write($"Id to a user with a UserName {user.UserName} is {user.Id}.");


            // var url = "https://www.ian.com/affiliatecenter/include/V2/ParentRegionList.zip";

            //// DownloadFile(url, runningStatus, importsFacade, import);

            //// todo Extract

      
            container.Register(Component.For<User>().Instance(user));

            var languagesRepository = container.Resolve<ILanguagesRepository>();

            var defaultLanguage = languagesRepository.Get(1033);
            if (defaultLanguage == null)
            {
                defaultLanguage = new Language
                {
                    Id = 1033,
                    EanLanguageCode = "en_US",
                    CreatorId = user.Id
                };
                languagesRepository.Add(defaultLanguage);
            }


            container.Register(
                Component.For<SharedProperties>()
                    .ImplementedBy<SharedProperties>()
                    .DependsOn(Dependency.OnValue("creatorId", user.Id), Dependency.OnValue("defaultLanguageId", defaultLanguage.Id))
            );

            
            using (var regionsImporter = container.Resolve<IImporter>(nameof(RegionsImporter)))
            {
                regionsImporter.Import(@"D:\Ean\ParentRegionList.txt");
            }
            

            //using (var countriesImporter = container.Resolve<IImporter>(nameof(CountriesImporter)))
            //{
            //    countriesImporter.Import(@"D:\Ean\CountryList.txt");
            //}
            

            //using (var neighborhoodsImporter = container.Resolve<IImporter>(nameof(NeighborhoodsImporter)))
            //{
            //    neighborhoodsImporter.Import(@"D:\Ean\NeighborhoodCoordinatesList.txt");
            //}
            
            //using (var citiesImporter = container.Resolve<IImporter>(nameof(RegionsTypesOfCitiesImporter)))
            //{
            //    citiesImporter.Import(@"D:\Ean\CityCoordinatesList.txt");
            //}
            
            //using (var pointsOfInterestImporter = container.Resolve<IImporter>(nameof(PointsOfInterestImporter)))
            //{
            //    pointsOfInterestImporter.Import(@"D:\Ean\PointsOfInterestCoordinatesList.txt");
            //}

            //using (var airportsImporter = container.Resolve<IImporter>(nameof(AirportsImporter)))
            //{
            //    airportsImporter.Import(@"D:\Ean\AirportCoordinatesList.txt");
            //}

            //using (var trainMetroStationsImporter = container.Resolve<IImporter>(nameof(TrainMetroStationsImporter)))
            //{
            //    trainMetroStationsImporter.Import(@"D:\Ean\TrainMetroStationCoordinatesList.txt");
            //}

            //using (var regionsCenterImporter = container.Resolve<IImporter>(nameof(RegionsCenterImporter)))
            //{
            //    regionsCenterImporter.Import(@"D:\Ean\RegionCenterCoordinatesList.txt");
            //}

            //using (var typesOfAccommodationsImporter = container.Resolve<IImporter>(nameof(TypesOfAccommodationsImporter)))
            //{
            //    typesOfAccommodationsImporter.Import(@"D:\Ean\PropertyTypeList.txt");

            //}
  
            //using (var chainsImporter = container.Resolve<IImporter>(nameof(ChainsImporter)))
            //{
            //    chainsImporter.Import(@"D:\Ean\ChainList.txt");
            //}
           
            //using (var accommodationsImporter = container.Resolve<IImporter>(nameof(AccommodationsImporter)))
            //{
            //    accommodationsImporter.Import(@"D:\Ean\ActivePropertyList.txt");
            //}
           

            //using (var descriptionsImporter = container.Resolve<IImporter>(nameof(DescriptionsImporter)))
            //{
            //    descriptionsImporter.Import(@"D:\Ean\PropertyDescriptionList.txt");
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
            
            container.Register(Component.For(typeof(IProvider)).ImplementedBy<FileImportProvider>().Named(nameof(FileImportProvider)));

            container.AddFacility<TypedFactoryFacility>();

            container.Register(Component.For<IFactoryOfRepositories>().AsFactory());
            
            container = RegisterImporters(container);
            
            return container;
        }


       private static WindsorContainer RegisterImporters(WindsorContainer container)
       {
            
           RegisterGeoImporters(container);

           RegisterAccoImporters(container);
            
           return container;
        }

        private static void RegisterAccoImporters(IWindsorContainer container)
        {
            container.Register(Component.For(typeof(IImporter)).ImplementedBy<TypesOfAccommodationsImporter>()
                .Named(nameof(TypesOfAccommodationsImporter)).Interceptors<IInterceptor>());

            container.Register(Component.For(typeof(IImporter)).ImplementedBy<ChainsImporter>()
                .Named(nameof(ChainsImporter)).Interceptors<IInterceptor>());

            container.Register(Component.For(typeof(IImporter)).ImplementedBy<AccommodationsImporter>()
                .Named(nameof(AccommodationsImporter)).Interceptors<IInterceptor>());

            container.Register(Component.For(typeof(IImporter)).ImplementedBy<DescriptionsImporter>()
                .Named(nameof(DescriptionsImporter)).Interceptors<IInterceptor>());
            

            container.Register(Component.For(typeof(IImporter)).ImplementedBy<PathsExtensionsCaptionsImporter>()
                .Named(nameof(PathsExtensionsCaptionsImporter)).Interceptors<IInterceptor>());
            
            container.Register(Component.For(typeof(IImporter)).ImplementedBy<PhotosOfAccommodationsImporter>()
                .Named(nameof(PhotosOfAccommodationsImporter)).Interceptors<IInterceptor>());


            container.Register(Component.For(typeof(IImporter)).ImplementedBy<TypesOfRoomsImporter>()
                .Named(nameof(TypesOfRoomsImporter)).Interceptors<IInterceptor>());


            container.Register(Component.For(typeof(IImporter)).ImplementedBy<LocalizedTypesOfRoomsImporter>()
                .Named(nameof(LocalizedTypesOfRoomsImporter)).Interceptors<IInterceptor>());

            container.Register(Component.For(typeof(IImporter)).ImplementedBy<RoomsTypesImagesImporter>()
                .Named(nameof(RoomsTypesImagesImporter)).Interceptors<IInterceptor>());


            container.Register(Component.For(typeof(IImporter))
                .ImplementedBy<PhotosOfAccommodationsToTypesOfRoomsImporter>()
                .Named(nameof(PhotosOfAccommodationsToTypesOfRoomsImporter)).Interceptors<IInterceptor>());


            container.Register(Component.For(typeof(IImporter)).ImplementedBy<AttributesImporter>()
                .Named(nameof(AttributesImporter)).Interceptors<IInterceptor>());

            container.Register(Component.For(typeof(IImporter))
                .ImplementedBy<LocalizedAttributesDefaultLanguageImporter>()
                .Named(nameof(LocalizedAttributesDefaultLanguageImporter)).Interceptors<IInterceptor>());

            container.Register(Component.For(typeof(IImporter))
                .ImplementedBy<AccommodationsToAttributesDefaultLanguageImporter>()
                .Named(nameof(AccommodationsToAttributesDefaultLanguageImporter)).Interceptors<IInterceptor>());
        }


        private static void RegisterGeoImporters(IWindsorContainer container)
        {
            container.Register(Component.For(typeof(IImporter)).ImplementedBy<RegionsImporter>().Named(nameof(RegionsImporter))
                .Interceptors<IInterceptor>());

            container.Register(Component.For(typeof(IImporter)).ImplementedBy<PointsOfInterestImporter>()
                .Named(nameof(PointsOfInterestImporter)).Interceptors<IInterceptor>());

            container.Register(Component.For(typeof(IImporter)).ImplementedBy<CountriesImporter>()
                .Named(nameof(CountriesImporter)).Interceptors<IInterceptor>());

            container.Register(Component.For(typeof(IImporter)).ImplementedBy<NeighborhoodsImporter>()
                .Named(nameof(NeighborhoodsImporter)).Interceptors<IInterceptor>());
            
            container.Register(Component.For(typeof(IImporter)).ImplementedBy<RegionsTypesOfCitiesImporter>()
                .Named(nameof(RegionsTypesOfCitiesImporter)).Interceptors<IInterceptor>());

            container.Register(Component.For(typeof(IImporter)).ImplementedBy<AirportsImporter>()
                .Named(nameof(AirportsImporter)).Interceptors<IInterceptor>());

            container.Register(Component.For(typeof(IImporter)).ImplementedBy<TrainMetroStationsImporter>()
                .Named(nameof(TrainMetroStationsImporter)).Interceptors<IInterceptor>());

            container.Register(Component.For(typeof(IImporter)).ImplementedBy<RegionsCenterImporter>()
                .Named(nameof(RegionsCenterImporter)).Interceptors<IInterceptor>());
        }


        public static void Write(object s)
        {
#if DEBUG
            Logger.Log(s.ToString());
#endif
        }
    }

}
