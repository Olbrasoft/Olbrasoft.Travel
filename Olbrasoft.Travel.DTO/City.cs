using System.Collections.Generic;

namespace Olbrasoft.Travel.DTO
{
    public class City : GeoWithCoordinates
    {
        public virtual ICollection<LocalizedCity> LocalizedCities { get; set; }

        public CityToCountry ToCountry { get; set; }

        public CityToSubClass ToSubClass { get; set; }

        public CityToRegion ToRegion { get; set; }
    }
}