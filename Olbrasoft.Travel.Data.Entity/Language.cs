using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.Data.Entity
{
    public class Language : CreatorInfo
    {
        [StringLength(5)]
        public string EanLanguageCode { get; set; }

        public virtual ICollection<LocalizedRegion> LocalizedRegions { get; set; }

        public virtual ICollection<LocalizedTypeOfAccommodation> LocalizedTypesOfAccommodations { get; set; }

        public virtual ICollection<LocalizedAccommodation> LocalizedAccommodations { get; set; }
        
        public virtual ICollection<Description> Descriptions { get; set; }

        public virtual ICollection<LocalizedCaption> LocalizedCaptions { get; set; }

        public virtual ICollection<LocalizedTypeOfRoom> LocalizedTypesOfRooms { get; set; }

        public virtual ICollection<LocalizedAttribute> LocalizedAttributes { get; set; }

        public virtual ICollection<AccommodationToAttribute> AccommodationsToAttributes { get; set; }
    }
}
