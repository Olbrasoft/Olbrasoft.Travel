using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class Language
    {
        [Key]
        public int Id { get; set; }
        
        [StringLength(5)]
        public string EanLanguageCode { get; set; }

        public int CreatorId { get; set; }

        public DateTime DateAndTimeOfCreation { get; set; }
        
        public User Creator { get; set; }

        public virtual ICollection<LocalizedContinent> LocalizedContinents { get; set; }

       // public virtual ICollection<LocalizedRegion> LocalizedRegions { get; set; }
       
       // public virtual ICollection<LocalizedPointOfInterest> LocalizedPointsOfInterest { get; set; }

    }
}
