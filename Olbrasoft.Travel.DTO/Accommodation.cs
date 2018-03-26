using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;

namespace Olbrasoft.Travel.DTO
{
    public class Accommodation : CreatorInfo, IHaveEanId<int>
    {
      
        public int SequenceNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Address { get; set; }

        public int CountryId { get; set; }

        [Required]
        public DbGeography CenterCoordinates { get; set; }

        public int TypeOfAccommodationId { get; set; }

        public int ChainId { get; set; }

        public int EanId { get; set; } = int.MinValue;

        public virtual Country Country { get; set; }

        public virtual TypeOfAccommodation TypeOfAccommodation { get; set; }

        public virtual Chain Chain { get; set; }

        public virtual ICollection<LocalizedAccommodation> LocalizedAccommodations { get; set; }

        public virtual ICollection<Description> Descriptions { get; set; }
    }
}
