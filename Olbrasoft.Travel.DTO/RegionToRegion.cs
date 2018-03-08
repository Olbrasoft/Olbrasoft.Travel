using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Olbrasoft.Travel.DTO
{
    public class RegionToRegion : TravelEntity
    {
        [Key]
        [Column(Order = 1)]
        public int RegionId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int ParentRegionId { get; set; }
        
        public virtual Region Region { get; set; }

        public virtual Region ParentRegion { get; set; }

        public int CreatorId { get; set; }

        public DateTime DateAndTimeOfCreation { get; set; }

        public virtual User Creator { get; set; }
    }
}
