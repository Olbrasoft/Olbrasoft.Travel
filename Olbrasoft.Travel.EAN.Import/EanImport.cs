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
using Castle.DynamicProxy;
using Castle.Facilities.TypedFactory;
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
            
            container.Register(Component.For<IImport>()
                .ImplementedBy<ParentRegionImporter>()
                .Named(nameof(ParentRegionImporter)));

            container.Register(Component.For<IImport>()
                .ImplementedBy<CountriesImporter>()
                .Named(nameof(CountriesImporter)));


            container.Register(Component.For<IImport>()
                .ImplementedBy<CitiesImporter>()
                .Named(nameof(CitiesImporter)));

            container.Register(Component.For<IImport>()
                .ImplementedBy<NeighborhoodsImporter>()
                .Named(nameof(NeighborhoodsImporter)));


            container.Register(Component.For<IImport>()
                .ImplementedBy<PointsOfInterestImporter>()
                .Named(nameof(PointsOfInterestImporter)));

            container.AddFacility<TypedFactoryFacility>();
            container.Register(Component.For<IFactoryOfRepositories>().AsFactory());

            //Logger.Log(container.Resolve<IBaseRepository<Continent>>().Count().ToString());
            //Logger.Log(container.Resolve<IFactoryOfRepositories>().Travel<Continent>().Count().ToString());


            //var repository = container.Resolve<IBaseRepository<RegionToRegion>>();
            //Console.ReadLine();

            var parentRegionImporter = container.Resolve<IImport>(nameof(ParentRegionImporter));
            parentRegionImporter.Import(@"D:\Ean\ParentRegionList.txt");

            var countriesImporter = container.Resolve<IImport>(nameof(CountriesImporter));
            countriesImporter.Import(@"D:\Ean\CountryList.txt");

            


            //var citiesImporter = container.Resolve<IImport>(nameof(CitiesImporter));
            //citiesImporter.Import(@"D:\Ean\CityCoordinatesList.Txt");

            //var neighborhoodsImporter = container.Resolve<IImport>(nameof(NeighborhoodsImporter));
            //neighborhoodsImporter.Import(@"D:\Ean\NeighborhoodCoordinatesList.Txt");

            //var pointsOfInterestImporter = container.Resolve<IImport>(nameof(PointsOfInterestImporter));
            //pointsOfInterestImporter.Import(@"D:\Ean\PointsOfInterestCoordinatesList.txt");





            //var path = @"D:/Ean/AirportCoordinatesList.txt";
            //var lines = File.ReadAllLines(path);
            //var parserFactory = container.Resolve<IParserFactory>();
            //var parser = parserFactory.Travel<Airport>(lines.FirstOrDefault());
            //foreach (var line in lines.Skip(1))
            //{

            //    if (parser.TryParse(line, out var airoport))
            //    {
            //        using (var context= new TravelContext())
            //        {
            //            context.Airports.Add(airoport);
            //            context.SaveChanges();
            //        }
            //    }            

            //}



            //loogerImports.Log("EanCountries Load.");
            //const string countryListFullPath = @"D:\Ean\ParentRegionList.Txt";
            //var eanCountries = new HashSet<DTO.Geography.Country>();
            //using (var reader = new StreamReader(countryListFullPath))
            //{
            //    var parserCountries = parserFactory.Travel<DTO.Geography.Country>(reader.ReadLine());

            //    while (!reader.EndOfStream)
            //    {
            //        if (parserCountries.TryParse(reader.ReadLine(), out var eanCountry))
            //        {
            //            eanCountries.Add(eanCountry);
            //        }
            //    }
            //}
            //loogerImports.Log("EanCountries Loaded.");


            //loogerImports.Log("EanCities Load.");
            //const string cityCityCoordinatesListFullPath = @"D:\Ean\CityCoordinatesList.Txt";
            //var eanCities = new HashSet<DTO.Geography.City>();
            //using (var reader = new StreamReader(cityCityCoordinatesListFullPath))
            //{
            //    var parserCities = parserFactory.Travel<DTO.Geography.City>(reader.ReadLine());

            //    while (!reader.EndOfStream)
            //    {
            //        if (parserCities.TryParse(reader.ReadLine(), out var eanCountry))
            //        {
            //            eanCities.Add(eanCountry);
            //        }
            //    }
            //}
            //loogerImports.Log("EanCities Loaded.");

            //var typesOfRegionsFacade = container.Resolve<ITypesOfRegionsFacade>();
            //var subClassesFacade = container.Resolve<ISubClassesFacade>();

            //loogerImports.Log("Regions from CityCoordinatesList Build");
            //var storedEanRegionsIds = regionsFacade.GetEanRegionsIds(true);
            //var typeOfRegionCity = typesOfRegionsFacade.Travel("City");
            //var subClassCity = subClassesFacade.Travel("city");

            //var regions = new HashSet<Region>();

            //foreach (var city in eanCities)
            //{
            //    if (storedEanRegionsIds.Contains(city.RegionID)) continue;
            //    var region = new Region
            //    {
            //        EanRegionId = city.RegionID,
            //        TypeOfRegionId = typeOfRegionCity.Id,
            //        SubClassId = subClassCity.Id,
            //        CreatorId = user.Id,
            //        DateAndTimeOfCreation = DateTime.Now

            //    };
            //    regions.Add(region);
            //}
            //loogerImports.Log("Regions from CityCoordinatesList Builded");

            ////loogerImports.Log("Regions from CityCoordinatesList Save");
            ////regionsFacade.BulkSave(regions);
            ////loogerImports.Log("Regions from CityCoordinatesList Saved");

            //var storedRegions = regionsFacade.GetMappingEanRegionIdsToRegions();

            //loogerImports.Log("Regions type of City for update coordinates Build.");
            //var cities = new List<Region>();

            //foreach (var eanCity in eanCities)
            //{
            //    if (storedRegions.TryGetValue(eanCity.RegionID, out var city))
            //    {
            //         city.Coordinates = CreatePoligon(eanCity.Coordinates);
            //    }
            //    else
            //    {
            //        loogerImports.Log($"Region EanRegionId: {eanCity.RegionID} not found in the Cities.");
            //    }
            //}
            //loogerImports.Log("Regions type of City for update coordinates Builded.");

            //var citiesImportOption = new ParentRegionImportOption(
            //    container.Resolve<IRegionsFacade>(),
            //    container.Resolve<ILoggingImports>(),
            //    container.Resolve<IParserFactory>()
            //    );





            Write("Imported");
#if DEBUG
            Console.ReadLine();
#endif
        }
        
        public static DbGeography ParsePolygon(string s)
        {
            var spl = s.Split(':');
            var pointsString = new StringBuilder();
          
                string lastPoint = null;

                foreach (var s1 in spl)
                {
                    var latLon = s1.Split(';');
                    var lotLanString = $"{latLon[1]} {latLon[0]}";
                    pointsString.Append(lotLanString + ",");
                    if (string.IsNullOrEmpty(lastPoint)) lastPoint = lotLanString;
                }

                pointsString.Append(lastPoint);

                return DbGeography.PolygonFromText($"POLYGON(({pointsString}))", 4326);
           
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
            //container.Register(Component.For<DbContext>().ImplementedBy<TravelContext>());

            container.Register(FromAssemblyNamed("Olbrasoft.Travel.BLL")
                .Where(type => type.Name.EndsWith("Facade"))
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
          
            container.Register(Component.For(typeof(IBaseRegionsRepository<>)).ImplementedBy(typeof(BaseRegionsRepository<>)));

            container.Register(Component.For(typeof(IBaseNamesRepository<>)).ImplementedBy(typeof(BaseNamesRepository<>)));

            container.Register(Component.For(typeof(IManyToManyRepository<>)).ImplementedBy(typeof(ManyToManyRepository<>)));

            container.Register(Component.For(typeof(ILocalizedRepository<>)).ImplementedBy(typeof(LocalizedRepository<>)));


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
