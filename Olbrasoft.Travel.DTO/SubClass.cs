using System.Collections.Generic;

namespace Olbrasoft.Travel.DTO
{
    public class SubClass : BaseName
    {
        public User Creator { get; set; }

        public virtual ICollection<Region> Regions { get; set; }

        public virtual ICollection<PointOfInterestToSubClass> PointsOfInterestToSubClasses { get; set; }


        //public virtual ICollection<PointOfInterest> PointsOfInterests { get; set; }
    }
}
