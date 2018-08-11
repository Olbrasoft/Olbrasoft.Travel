using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.Data.Entity
{
    public class BaseName : CreatorInfo, IHaveName
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
