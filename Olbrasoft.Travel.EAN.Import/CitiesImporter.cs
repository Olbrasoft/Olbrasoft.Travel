using Olbrasoft.Travel.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;
using Olbrasoft.Travel.DTO;
using City = Olbrasoft.Travel.EAN.DTO.Geography.City;

namespace Olbrasoft.Travel.EAN.Import
{
   
    internal class CitiesImporter : Importer<City>
    {
        private readonly object _lockMe = new object();

        public CitiesImporter(ImportOption option) : base(option)
        {

        }

        public override void ImportBatch(City[] eanCities)
        {
            var citiesRepository = FactoryOfRepositories.BaseRegions<Travel.DTO.City>();
            var typeName = typeof(Travel.DTO.City).Name;

            Logger.Log($"{typeName} Build.");
            var cities = BuildCitiesOrNeighborhoods<Travel.DTO.City>(eanCities, CreatorId);
            Logger.Log(cities.Length.ToString());

            Logger.Log($"{typeName} Save.");
            citiesRepository.BulkSave(cities);
            Logger.Log($"{typeName} Saved.");

            var eanRegionIdsToIds = citiesRepository.EanRegionIdsToIds;

            typeName = typeof(LocalizedCity).Name;
            Logger.Log($"{typeName} Build.");
            var localizedCities = BuildLocalizedCitiesOrNeighborhoods<LocalizedCity>(eanCities, eanRegionIdsToIds, DefaultLanguageId, CreatorId);
            Logger.Log($"{typeName} Builded:{localizedCities.Length}.");

            Logger.Log($"{typeName} Save.");
            FactoryOfRepositories.Localized<LocalizedCity>().BulkSave(localizedCities);
            Logger.Log($"{typeName} Saved");
            
        }

        
       

        //private Travel.DTO.City[] BuildCities(IEnumerable<City> eanCities,  int creatorId)
        //{
        //    //var eanRegionIdsToIds = repository.EanRegionIdsToIds;
        //    // var cities = new Dictionary<long, Travel.DTO.City>();

        //    // var cities=new List<Travel.DTO.City>();
        //    // Parallel.ForEach(sourceCollection, item => BuildCity(item));
        //    var cities = new Queue<Travel.DTO.City>();
        //    Parallel.ForEach(eanCities, eanCity =>
        //    {
        //        var city = new Travel.DTO.City
        //        {
        //            EanRegionId = eanCity.RegionID,
        //            Coordinates = CreatePoligon(eanCity.Coordinates),
        //            CreatorId = creatorId
        //        };

        //        lock (_lockMe)
        //        {
        //            cities.Enqueue(city);
        //        }

        //    });


        //    //foreach (var eanCity in eanCities)
        //    //{
        //    //    if (cities.ContainsKey(eanCity.RegionID)) continue;

        //    //    var city = new Travel.DTO.City
        //    //    {
        //    //        EanRegionId = eanCity.RegionID,
        //    //        Coordinates = CreatePoligon(eanCity.Coordinates),
        //    //        CreatorId = creatorId

        //    //    };

        //    //    cities.Add(eanCity.RegionID, city);
        //    //}

        //    // return cities.Values.ToArray();

        //    return cities.ToArray();
        //}


    }



}
