using System.Data.Entity.Spatial;

namespace Olbrasoft.Travel.DTO
{
    public class Neighborhood : BaseRegionCoordinates
    {
        public virtual User Creator { get; set; }
    }
    
}