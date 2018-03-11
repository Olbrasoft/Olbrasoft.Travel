using System.Collections.Generic;
using System.Data.Entity.Spatial;

namespace Olbrasoft.Travel.DTO
{
    public class City : BaseRegion
    {
        public DbGeography Coordinates { get; set; }
        public virtual User Creator { get; set; }
    }

  
}