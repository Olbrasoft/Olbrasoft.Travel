using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.Data.Entity
{
    public class LocalizedTypeOfAccommodation : Localized
    {
        [Required]
        [StringLength(256)]
        public virtual string Name { get; set; }

        public virtual TypeOfAccommodation TypeOfAccommodation { get; set; }
    }
}