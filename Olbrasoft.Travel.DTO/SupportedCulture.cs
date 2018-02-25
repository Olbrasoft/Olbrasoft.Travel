using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class SupportedCulture : TravelEntity
    {

        [Key]
        public int Id { get; set; }
        public virtual ICollection<Description> Descriptions { get; set; }
        public virtual ICollection<LocalizedCategory> LocalizedCategories { get; set;}
        public virtual ICollection<LocalizedAccommodation> LocalizedAccommodations { get; set; }
        public virtual ICollection<LocalizedRegion> LocalizedRegions { get; set; }

    }
}