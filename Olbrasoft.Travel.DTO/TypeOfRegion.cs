using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class TypeOfRegion 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public int CreatorId { get; set; }

        public DateTime DateAndTimeOfCreation { get; set; }

        public User Creator { get; set; }
        
        public virtual ICollection<Region> Regions { get; set; }
    }
}
