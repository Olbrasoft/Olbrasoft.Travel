using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class BaseLocalizedRegion : Localized
    {
        [Required]
        [StringLength(255)]
        public virtual string Name { get; set; }
        
    }


}