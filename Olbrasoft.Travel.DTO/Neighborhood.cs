using System.Collections.Generic;

namespace Olbrasoft.Travel.DTO
{
    public class Neighborhood : GeoWithCoordinates
    {
        public virtual ICollection<LocalizedNeighborhood> LocalizedNeighborhoods { get; set; }
    }
}