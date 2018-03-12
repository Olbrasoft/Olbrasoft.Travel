using System.Data.Entity.Spatial;

namespace Olbrasoft.Travel.DTO
{
    public class BaseRegionWithCoordinates:BaseRegion
    {
        public DbGeography Coordinates { get; set; }
    }
}