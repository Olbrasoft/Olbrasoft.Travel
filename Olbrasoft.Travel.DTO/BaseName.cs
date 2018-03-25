using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class BaseName : CreatorInfo
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
