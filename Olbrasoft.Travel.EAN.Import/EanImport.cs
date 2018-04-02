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
using Olbrasoft.Travel.EAN.DTO.Geography;
using Olbrasoft.Travel.EAN.DTO.Property;

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
                .DependsOn(Dependency.OnValue("creatorId", user.Id),Dependency.OnValue("defaultLanguageId", defaultLanguage.Id))
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

            container.Register(Component.For(typeof(IImport<Travel.EAN.DTO.Property.PathToHotelImage>))
                .ImplementedBy<PathsToImagesOfHotelsImporter>()
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

            //var pathsToImagesOfHotelsImporter = container.Resolve<IImport<PathToHotelImage>>();
            //pathsToImagesOfHotelsImporter.Import($@"D:\Ean\HotelImageList.txt");


            //var imagesOfHotelsImporter = container.Resolve<IImport<HotelImage>>();
            //imagesOfHotelsImporter.Import($@"D:\Ean\HotelImageList.txt");


            //var develepmentRoomsTypesImporter = new Development.DevelopmentRoomsTypesImporter(container.Resolve<ImportOption>());
            //develepmentRoomsTypesImporter.Import(@"D:\Ean\RoomTypeList.txt");


            //var roomsTypesImporter = container.Resolve<IImport<RoomType>>();
            //roomsTypesImporter.Import(@"D:\Ean\RoomTypeList.txt");




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
         container.Register(Component.For<ILoggingImports>().ImplementedBy<ImportsLogger>());
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
