using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class SubClass 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int CreatorId { get; set; }

        public DateTime DateAndTimeOfCreation { get; set; }

        public User Creator { get; set; }

        public virtual ICollection<Region> Regions { get; set; }

        public virtual ICollection<PointOfInterest> PointsOfInterests { get; set; }
    }
}
