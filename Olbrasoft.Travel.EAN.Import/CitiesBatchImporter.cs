using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{

    internal class CitiesBatchImporter : BatchImporter<CityCoordinates>
    {

        public CitiesBatchImporter(ImportOption option) : base(option)
        {

        }

        public override void ImportBatch(CityCoordinates[] eanCitiesCoordinates)
        {
            var citiesRepository = FactoryOfRepositories.Geo<City>();

            LogBuild<City>();
            var cities = BuildCitiesOrNeighborhoods<City>(eanCitiesCoordinates, CreatorId);
            LogBuilded(cities.Length);

            LogSave<City>();
            citiesRepository.BulkSave(cities);
            LogSaved<City>();

            var eanRegionIdsToIds = citiesRepository.EanIdsToIds;

            LogBuild<LocalizedCity>();
            var localizedCities = BuildLocalizedRegions<LocalizedCity>(
                eanCitiesCoordinates,
                eanRegionIdsToIds, DefaultLanguageId, CreatorId);
            LogBuilded(localizedCities.Length);

            LogSave<LocalizedCity>();
            FactoryOfRepositories.Localized<LocalizedCity>().BulkSave(localizedCities);
            LogSaved<LocalizedCity>();
        }
        


        //private Travel.DTO.CityCoordinates[] BuildCities(IEnumerable<CityCoordinates> eanCitiesCoordinates,  int creatorId)
        //{
        //    //var eanRegionIdsToIds = repository.EanAirportIdsToIds;
        //    // var cities = new Dictionary<long, Travel.DTO.CityCoordinates>();

        //    // var cities=new List<Travel.DTO.CityCoordinates>();
        //    // Parallel.ForEach(sourceCollection, item => BuildCity(item));
        //    var cities = new Queue<Travel.DTO.CityCoordinates>();
        //    Parallel.ForEach(eanCitiesCoordinates, eanCity =>
        //    {
        //        var city = new Travel.DTO.CityCoordinates
        //        {
        //            EanId = eanCity.RegionID,
        //            Coordinates = CreatePoligon(eanCity.Coordinates),
        //            CreatorId = creatorId
        //        };

        //        lock (_lockMe)
        //        {
        //            cities.Enqueue(city);
        //        }

        //    });


        //    //foreach (var eanCity in eanCitiesCoordinates)
        //    //{
        //    //    if (cities.ContainsKey(eanCity.RegionID)) continue;

        //    //    var city = new Travel.DTO.CityCoordinates
        //    //    {
        //    //        EanId = eanCity.RegionID,
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
