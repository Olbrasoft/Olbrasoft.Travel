using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class LocalizedTypeOfRoom : Localized
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual TypeOfRoom TypeOfRoom { get; set; }
    }
}
