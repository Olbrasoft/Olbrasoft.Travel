using System;
using System.Collections.Generic;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{
    internal class AirportsBatchImporter : BatchImporter<AirportCoordinates>
    {
        public AirportsBatchImporter(ImportOption option) : base(option)
        {
        }

        public override void ImportBatch(AirportCoordinates[] eanAirportsCoordinates)
        {

            var regionsRepository = FactoryOfRepositories.Regions();

            var typeOfRegionAirportId = FactoryOfRepositories.BaseNames<TypeOfRegion>().GetId("Airport");

            var subClassAirportId = FactoryOfRepositories.BaseNames<SubClass>().GetId("airport");


            IReadOnlyDictionary<long, int> eanIdsToIds;

            LogBuild<Region>();
            var regions = BuildRegions(eanAirportsCoordinates, CreatorId);
            var count = regions.Length;
            LogBuilded(count);

            if (count > 0)
            {
                LogSave<Region>();
                regionsRepository.BulkSave(regions, r => r.Coordinates, r => r.EanId);
                LogSaved<Region>();

                eanIdsToIds = regionsRepository.EanIdsToIds;

                LogBuild<LocalizedRegion>();
                var localizedRegions = BuildLocalizedRegions(eanAirportsCoordinates, eanIdsToIds,
                    DefaultLanguageId, CreatorId);
                count = localizedRegions.Length;
                LogBuilded(count);

                if (count > 0)
                {
                    LogSave<LocalizedRegion>();
                    FactoryOfRepositories.Localized<LocalizedRegion>().BulkSave(localizedRegions);
                    LogSaved<LocalizedRegion>();
                }
            }
            else
            {
                eanIdsToIds = regionsRepository.EanIdsToIds;
            }

            LogBuild<Airport>();
            var airports = BuildAirports(eanAirportsCoordinates, eanIdsToIds, CreatorId);
            count = airports.Length;
            LogBuilded(count);

            if (count > 0)
            {
                LogSave<Airport>();
                FactoryOfRepositories.AdditionalRegionsInfo<Airport>().BulkSave(airports);
                LogSaved<Airport>();
            }

            LogBuild<RegionToType>();
            var regionsToTypes =
                BuildRegionsToTypes(eanAirportsCoordinates, eanIdsToIds, typeOfRegionAirportId, subClassAirportId, CreatorId);
            count = regionsToTypes.Length;
            LogBuilded(count);

            if (count > 0)
            {
                LogSave<RegionToType>();
                FactoryOfRepositories.RegionsToTypes().BulkSave(regionsToTypes);
                LogSaved<RegionToType>();
            }

            LogBuild<RegionToRegion>();
            var regionsToRegions = BuildRegionsToregions(eanAirportsCoordinates, eanIdsToIds,
                regionsRepository.EanIdsToIds, FactoryOfRepositories.AdditionalRegionsInfo<Travel.DTO.Country>().CodesToIds, CreatorId);
            count = regionsToRegions.Length;
            LogBuilded(count);


            if (count <= 0) return;
            LogSave<RegionToRegion>();
            FactoryOfRepositories.ManyToMany<RegionToRegion>().BulkSave(regionsToRegions);
            LogSaved<RegionToRegion>();

        }

        private static RegionToType[] BuildRegionsToTypes(IEnumerable<AirportCoordinates> eanAirportsCoordinates,
            IReadOnlyDictionary<long, int> eanAirportIdsToIds,
            int typeOfRegionAirportId,
            int subClasAirportId,
            int creatorId
            )
        {
            var regionsToTypes = new Queue<RegionToType>();

            foreach (var entity in eanAirportsCoordinates)
            {
                if (!eanAirportIdsToIds.TryGetValue(entity.AirportID, out var id)) continue;

                var regionToType = new RegionToType
                {
                    Id = id,
                    ToId = typeOfRegionAirportId,
                    SubClassId = subClasAirportId,
                    CreatorId = creatorId
                };

                regionsToTypes.Enqueue(regionToType);
            }
            return regionsToTypes.ToArray();
        }

        private static RegionToRegion[] BuildRegionsToregions(IEnumerable<AirportCoordinates> eanAirportsCoordinates,
            IReadOnlyDictionary<long, int> eanAirportIdsToIds,
            IReadOnlyDictionary<long, int> eanRegionIdsToIds,
            IReadOnlyDictionary<string, int> eanCountryCodeToIds,
            int creatorId
            )
        {
            var regionsToregions = new Queue<RegionToRegion>();

            foreach (var eanAirport in eanAirportsCoordinates)
            {

                if (!eanAirportIdsToIds.TryGetValue(eanAirport.AirportID, out var id)) continue;

                if (eanAirport.MainCityID != null)
                {
                    var cityId = (long)eanAirport.MainCityID;

                    if (eanRegionIdsToIds.TryGetValue(cityId, out var toId))
                    {
                        var regionToRegion = new RegionToRegion
                        {
                            Id = id,
                            ToId = toId,
                            CreatorId = creatorId
                        };

                        regionsToregions.Enqueue(regionToRegion);
                    }
                }

                if (!eanCountryCodeToIds.TryGetValue(eanAirport.CountryCode, out var toId1)) continue;
                {
                    var regionToRegion = new RegionToRegion
                    {
                        Id = id,
                        ToId = toId1,
                        CreatorId = creatorId
                    };

                    regionsToregions.Enqueue(regionToRegion);
                }

            }

            return regionsToregions.ToArray();
        }

        private static Airport[] BuildAirports(IEnumerable<AirportCoordinates> eanAirportsCoordinates,
            IReadOnlyDictionary<long, int> eanAirportIdsToIds,
            int creatorId
            )
        {
            var airports = new Queue<Airport>();

            foreach (var eanAirport in eanAirportsCoordinates)
            {
                if (!eanAirportIdsToIds.TryGetValue(eanAirport.AirportID, out var id)) continue;

                var airport = new Airport()
                {
                    Id = id,
                    Code = eanAirport.AirportCode,
                    CreatorId = creatorId
                };

                airports.Enqueue(airport);
            }

            return airports.ToArray();
        }

        private static LocalizedRegion[] BuildLocalizedRegions(IEnumerable<AirportCoordinates> eanAirportsCoordinates,
        IReadOnlyDictionary<long, int> eanAirportIdsToIds,
        int languageId,
        int creatorId
        )
        {
            var localizedRegions = new Queue<LocalizedRegion>();

            foreach (var eanAirport in eanAirportsCoordinates)
            {
                if (!eanAirportIdsToIds.TryGetValue(eanAirport.AirportID, out var id)) continue;

                var localizedRegion = new LocalizedRegion()
                {
                    Id = id,
                    LanguageId = languageId,
                    Name = eanAirport.AirportName,
                    CreatorId = creatorId
                };

                localizedRegions.Enqueue(localizedRegion);
            }

            return localizedRegions.ToArray();
        }


        private static Region[] BuildRegions(IEnumerable<AirportCoordinates> eanAirportCoordinates,
         int creatorId
        )
        {
            var regions = new Queue<Region>();

            foreach (var eanAirport in eanAirportCoordinates)
            {
                var region = new Region
                {
                    CenterCoordinates = CreatePoint(eanAirport.Latitude, eanAirport.Longitude),
                    EanId = eanAirport.AirportID,
                    CreatorId = creatorId
                };

                regions.Enqueue(region);
            }

            return regions.ToArray();
        }

    }
}


