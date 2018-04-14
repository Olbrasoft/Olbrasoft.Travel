using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class LocalizedAttribute : Localized
    {
        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        public Attribute Attribute { get; set; }
    }
}
