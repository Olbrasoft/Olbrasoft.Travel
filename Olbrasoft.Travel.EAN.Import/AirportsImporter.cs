using System;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Import
{
    class AirportsImporter:Importer<Travel.EAN.DTO.Geography.Airport>
    {
        public AirportsImporter(ImportOption option) : base(option)
        {
        }


        public override void ImportBatch(Airport[] parentRegions)
        {

            throw new NotImplementedException();
        }

        
        
    }
}


