using System.Collections.Generic;

namespace Olbrasoft.Travel.DTO
{
    public class City : BaseRegionWithCoordinates
    {
       public virtual User Creator { get; set; }

       public virtual ICollection<LocalizedCity> LocalizedCities { get; set; }
    }
}