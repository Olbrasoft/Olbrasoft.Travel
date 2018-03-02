using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olbrasoft.Travel.DTO
{
    public class PointOfInterest
    {
        [Key]
        public int Id { get; set; }

        public bool Shadow { get; set; }

        public int? SubClassId { get; set; }

        public long? EanRegionId { get; set; }

        public DbGeography Coordinates { get; set; }

        public int CreatorId { get; set; }

        public DateTime DateAndTimeOfCreation { get; set; }

        public SubClass SubClass { get; set; }

        public User Creator { get; set; }

        public ICollection<LocalizedPointOfInterest> LocalizedPointsOfInterest { get; set; }
        public ICollection<PointOfInterestToPointOfInterest> ToParentPointsOfInterest { get; set; }
        public ICollection<PointOfInterestToPointOfInterest> ToChildPointsOfInterest { get; set; }

        public virtual ICollection<PointOfInterestToRegion> PointsOfInterestToRegions { get; set; }

       // public virtual ICollection<Region> Regions { get; set; }
    }
}
