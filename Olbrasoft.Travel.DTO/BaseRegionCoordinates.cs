using System.Data.Entity.Spatial;

namespace Olbrasoft.Travel.DTO
{
    public class BaseRegionCoordinates:BaseRegion
    {
        public DbGeography Coordinates { get; set; }
    }
}