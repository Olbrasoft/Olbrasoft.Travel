using System.Collections.Generic;
using System.Data.Entity.Spatial;

namespace Olbrasoft.Travel.DTO
{
    public class Region : BaseRegion
    {
        public DbGeography Coordinates { get; set; }

        public User Creator { get; set; }

        public TypeOfRegion TypeOfRegion { get; set; }

        public SubClass SubClass { get; set; }

        public ICollection<LocalizedRegion> LocalizedRegions { get; set; }

        public ICollection<RegionToRegion> ToParentRegions { get; set; }

        public ICollection<RegionToRegion> ToChildRegions { get; set; }

        public virtual ICollection<PointOfInterestToRegion> PointsOfInterestToRegions { get; set; }

        // public virtual ICollection<PointOfInterest> PointsOfInterest { get; set; }
        //public ICollection<Region> ParentRegions { get; set; }
        //public ICollection<Region> ChildRegions { get; set; }
    }
}
