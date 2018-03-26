using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class BaseName : CreatorInfo, IHaveName
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
