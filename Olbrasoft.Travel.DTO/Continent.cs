using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class Continent 
    {
        [Key]
        public int Id { get; set; }

        public long EanRegionId { get; set; } = long.MinValue;

        public int CreatorId { get; set; }

        public DateTime DateAndTimeOfCreation { get; set; }

        public virtual User Creator { get; set; }

        public virtual ICollection<LocalizedContinent> LocalizedContinents { get; set; }
    }
}
