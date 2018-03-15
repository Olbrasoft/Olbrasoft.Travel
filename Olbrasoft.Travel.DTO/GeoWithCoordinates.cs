using System.Data.Entity.Spatial;

namespace Olbrasoft.Travel.DTO
{
    public class GeoWithCoordinates : Geo
    {
        public DbGeography Coordinates { get; set; }
    }
}