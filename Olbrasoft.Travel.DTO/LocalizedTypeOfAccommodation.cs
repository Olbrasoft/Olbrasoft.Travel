using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class LocalizedTypeOfAccommodation : Localized
    {
        [Required]
        [StringLength(256)]
        public virtual string Name { get; set; }

        public virtual TypeOfAccommodation TypeOfAccommodation { get; set; }
    }
}