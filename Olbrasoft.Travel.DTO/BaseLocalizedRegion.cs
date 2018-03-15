using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class BaseLocalizedRegion : BaseLocalized
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        
    }
}