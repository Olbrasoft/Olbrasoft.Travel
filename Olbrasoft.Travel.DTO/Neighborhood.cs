using System.Collections.Generic;

namespace Olbrasoft.Travel.DTO
{
    public class Neighborhood : BaseRegionWithCoordinates
    {
        public virtual User Creator { get; set; }
        public virtual ICollection<LocalizedNeighborhood> LocalizedNeighborhoods { get; set; }
    }
    
}