using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class LocalizedCountry: BaseLocalized
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public User Creator { get; set; }

        public virtual Country Country { get; set; }

        public virtual Language Language { get; set; }
    }
}