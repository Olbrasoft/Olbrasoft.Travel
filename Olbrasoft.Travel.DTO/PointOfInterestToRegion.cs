using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Olbrasoft.Travel.DTO
{
    public class PointOfInterestToRegion
    {
        [Key]
        [Column(Order = 1)]
        public int PointOfInterestId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int RegionId { get; set; }
        
        public virtual Region Region { get; set; }
        
        public virtual PointOfInterest PointOfInterest { get; set; }

        public int CreatorId { get; set; }

        public DateTime DateAndTimeOfCreation { get; set; }

        public virtual User Creator { get; set; }
    }
}