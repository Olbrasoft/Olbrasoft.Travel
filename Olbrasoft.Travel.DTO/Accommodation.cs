using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;

namespace Olbrasoft.Travel.DTO
{
  
    public partial class Accommodation:MappedEntity
    {
        [Required]
        [StringLength(70)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Address { get; set; }

        [Required]
        public DbGeography Coordinates { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int ChainId { get; set; }
        
        public virtual Category Category { get; set; }

        public virtual Chain Chain { get; set; }

        public virtual ICollection<Description> Descriptions { get; set; }
        public virtual ICollection<LocalizedAccommodation> LocalizedAccommodations { get; set; }
    }
}
