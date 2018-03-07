using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{
    class AirportsImporter:Importer<Travel.EAN.DTO.Geography.Airport>
    {
        public AirportsImporter(ImportOption option) : base(option)
        {
        }
         

        protected override void ImportBatch(IEnumerable<Airport> eanAirports)
        {

            throw new NotImplementedException();
        }

        private static Travel.DTO.Airport[] BuildAirports(

            IEnumerable<Airport> eanAirports,
            IDictionary<long, Travel.DTO.BaseAirport> storedBaseAirports
            
            )
        {
            throw new NotImplementedException();
        }

        
    }
}


