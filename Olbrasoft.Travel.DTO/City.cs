using System.Collections.Generic;

namespace Olbrasoft.Travel.DTO
{
    public class City : GeoWithCoordinates
    {
       public virtual ICollection<LocalizedCity> LocalizedCities { get; set; }
    }
}