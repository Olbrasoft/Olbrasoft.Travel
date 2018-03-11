using System.Collections.Generic;

namespace Olbrasoft.Travel.DTO
{
    public class City : BaseRegionCoordinates
    {
       public virtual User Creator { get; set; }
    }
}