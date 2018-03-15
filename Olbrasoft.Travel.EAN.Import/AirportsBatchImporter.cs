using System;
using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{
    class AirportsBatchImporter:BatchImporter<AirportCoordinates>
    {
        public AirportsBatchImporter(ImportOption option) : base(option)
        {
        }
        
        public override void ImportBatch(AirportCoordinates[] eanAirportsCoordinates)
        {
            ImportAirports(eanAirportsCoordinates,FactoryOfRepositories.Geo<Airport>(),CreatorId);
           
        }

        void ImportAirports(IEnumerable<AirportCoordinates> eanAirportsCoordinates, IMapToPartnersRepository<Airport, long> repository, int creatorId  )
        {
            LogBuild<Airport>();
            var airports = BuildAirports(eanAirportsCoordinates, creatorId);
            LogBuilded(airports.Length);

            LogSave<Airport>();
            repository.BulkSave(airports);
            LogSaved<Airport>();
        }

        private static Airport[] BuildAirports(IEnumerable<AirportCoordinates> eanAirportsCoordinates, int creatorId)
        {
            return eanAirportsCoordinates.Select(
                p => new Airport
                {
                    EanId = p.AirportID,
                    Code = p.AirportCode,
                    Coordinates = CreatePoint(p.Latitude, p.Longitude),
                    CreatorId = creatorId

                }
            ).ToArray();

        }
    }
}


