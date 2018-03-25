using System.Collections.Generic;

namespace Olbrasoft.Travel.DTO
{
    public class TypeOfRegion : BaseName
    {
        public ICollection<RegionToType> RegionsToTypes { get; set; }
        

       //public virtual ICollection<Region> Regions { get; set; }
    }
}
