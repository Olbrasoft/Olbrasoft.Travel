using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class Airport : CreatorInfo
    {
        [Required]
        [StringLength(3)]
        public string Code { get; set; }

        public virtual Region Region { get; set; }
    }
}