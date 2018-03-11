using Olbrasoft.Travel.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;
using Olbrasoft.Travel.DTO;
using City = Olbrasoft.Travel.EAN.DTO.Geography.City;

namespace Olbrasoft.Travel.EAN.Import
{
    
    internal class NeighborhoodsImporter : Importer<DTO.Geography.Neighborhood>
    {
        public NeighborhoodsImporter(ImportOption option) : base(option)
        {
         
        }

        public override void ImportBatch(DTO.Geography.Neighborhood[] eanNeighBorhoods)
        {
            var neighborhoodsRepository = FactoryOfRepositories.BaseRegions<Travel.DTO.Neighborhood>();
            var typeName = typeof(Travel.DTO.Neighborhood).Name;

            Logger.Log($"{typeName} Build.");
            var neighborhoods = BuildCitiesOrNeighborhoods<Travel.DTO.Neighborhood>(eanNeighBorhoods, CreatorId);
            Logger.Log(neighborhoods.Length.ToString());

            Logger.Log($"{typeName} Save.");
            neighborhoodsRepository.BulkSave(neighborhoods);
            Logger.Log($"{typeName} Saved.");
        }
    }


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
            var localizedCities = BuildLocalizedCities(eanCities, eanRegionIdsToIds, DefaultLanguageId, CreatorId);
            Logger.Log($"{typeName} Builded:{localizedCities.Length}.");

            Logger.Log($"{typeName} Save.");
            FactoryOfRepositories.Localized<LocalizedCity>().BulkSave(localizedCities);
            Logger.Log($"{typeName} Saved");
            
        }

        private LocalizedCity[] BuildLocalizedCities(City[] eanCities, IReadOnlyDictionary<long, int> eanRegionIdsToIds, int languageId, int creatorId)
        {
            var localizedCities = new Queue<LocalizedCity>();

            Parallel.ForEach(eanCities, eanCity =>
            {
                if (!eanRegionIdsToIds.TryGetValue(eanCity.RegionID, out var id)) return;
                var localizedCity = new LocalizedCity
                {
                    Id = id,
                    LanguageId = languageId,
                    Name = eanCity.RegionName,
                    CreatorId = creatorId
                };
                lock (_lockMe)
                {
                    localizedCities.Enqueue(localizedCity);
                }
            });           
            return localizedCities.ToArray();
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
