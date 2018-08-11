using System.Collections.Generic;

namespace Olbrasoft.Travel.Data.Entity
{
    public class SubClass : BaseName
    {
         
        public virtual ICollection<RegionToType> RegionsToTypes { get; set; }

        // public virtual ICollection<RegionToSubClass> RegionsToSubClasses { get; set; }
        
        //public virtual ICollection<Region> Regions { get; set; }

        // public virtual ICollection<CityToSubClass> CitiesToSubClasses { get; set; }

        // public virtual ICollection<PointOfInterestToSubClass> PointsOfInterestToSubClasses { get; set; }
        
        //public virtual ICollection<PointOfInterest> PointsOfInterests { get; set; }
    }
}
