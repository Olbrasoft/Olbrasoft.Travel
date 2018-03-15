﻿using System.Collections.Generic;

namespace Olbrasoft.Travel.DTO
{
    public class Region : BaseGeo
    {
        public int TypeOfRegionId { get; set; }

        public int? SubClassId { get; set; }

        public TypeOfRegion TypeOfRegion { get; set; }

        public SubClass SubClass { get; set; }

        public virtual User Creator { get; set; }

        public ICollection<LocalizedRegion> LocalizedRegions { get; set; }
        public ICollection<RegionToRegion> ToParentRegions { get; set; }
        public ICollection<RegionToRegion> ToChildRegions { get; set; }


        // public virtual ICollection<PointOfInterestToRegion> PointsOfInterestToRegions { get; set; }



        // public virtual ICollection<PointOfInterest> PointsOfInterest { get; set; }
        //public ICollection<Region> ParentRegions { get; set; }
        //public ICollection<Region> ChildRegions { get; set; }
    }
}
