using System.Data.Entity.Spatial;

namespace Olbrasoft.Travel.DTO
{
    public class GeoWithCoordinates : BaseGeo
    {
        public DbGeography Coordinates { get; set; }
    }
}