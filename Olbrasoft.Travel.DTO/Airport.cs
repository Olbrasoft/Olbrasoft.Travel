using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class Airport : CreatorInfo , IAdditionalRegionInfo
    {
        [Required]
        [StringLength(3)]
        public string Code { get; set; }

        public virtual Region Region { get; set; }

        public virtual ICollection<Accommodation> Accommodations { get; set; }
    }
}