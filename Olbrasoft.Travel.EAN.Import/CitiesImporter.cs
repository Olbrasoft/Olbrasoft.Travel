using System;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.EAN.DTO.Geography;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Configuration;
using System.Threading;
using System.Threading.Tasks;

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
            var cities = BuildCities(eanCities, citiesRepository, CreatorId);
            Logger.Log(cities.Length.ToString());

            Logger.Log($"{typeName} Save.");
            citiesRepository.BulkSave(cities);
            Logger.Log($"{typeName} Saved.");

            var eanRegionIdsToIds = citiesRepository.EanRegionIdsToIds;

            //typeName = typeof(Travel.DTO.LocalizedC).Name;
            Logger.Log($"{typeName} Build.");
            //var localizedCountries = BuildLocalizedCountries(eanCountries, eanRegionIdsToIds, DefaultLanguageId, CreatorId);
            //Logger.Log($"{typeName} Builded: {localizedCountries.Length}.");

        }

        private Travel.DTO.City[] BuildCities(IEnumerable<City> eanCities, IBaseRegionsRepository<Travel.DTO.City> repository, int creatorId)
        {
            var eanRegionIdsToIds = repository.EanRegionIdsToIds;
            // var cities = new Dictionary<long, Travel.DTO.City>();

            // var cities=new List<Travel.DTO.City>();
            // Parallel.ForEach(sourceCollection, item => BuildCity(item));
            var cities = new Queue<Travel.DTO.City>();
            Parallel.ForEach(eanCities, eanCity =>
            {
                var city = new Travel.DTO.City
                {
                    EanRegionId = eanCity.RegionID,
                    Coordinates = CreatePoligon(eanCity.Coordinates),
                    CreatorId = creatorId
                };

                lock (_lockMe)
                {
                    cities.Enqueue(city);
                }

            });


            //foreach (var eanCity in eanCities)
            //{
            //    if (cities.ContainsKey(eanCity.RegionID)) continue;

            //    var city = new Travel.DTO.City
            //    {
            //        EanRegionId = eanCity.RegionID,
            //        Coordinates = CreatePoligon(eanCity.Coordinates),
            //        CreatorId = creatorId

            //    };

            //    cities.Add(eanCity.RegionID, city);
            //}

            // return cities.Values.ToArray();

            return cities.ToArray();
        }


    }



}
