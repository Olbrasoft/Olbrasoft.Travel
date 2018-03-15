﻿using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olbrasoft.Travel.DTO
{
    public class PointOfInterest : GeoWithCoordinates
    {
        //public SubClass SubClass { get; set; }

        public bool Shadow { get; set; }

        public virtual User Creator { get; set; }

        public ICollection<LocalizedPointOfInterest> LocalizedPointsOfInterest { get; set; }


        //public ICollection<PointOfInterestToPointOfInterest> ToParentPointsOfInterest { get; set; }
        //public ICollection<PointOfInterestToPointOfInterest> ToChildPointsOfInterest { get; set; }
        //public virtual ICollection<PointOfInterestToRegion> PointsOfInterestToRegions { get; set; }

       // public virtual PointOfInterestToSubClass ToSubClass { get; set; }


        // public virtual ICollection<Region> Regions { get; set; }
    }
}
