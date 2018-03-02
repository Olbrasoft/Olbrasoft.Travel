using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Olbrasoft.Travel.DTO
{
    public class LocalizedCategory
    {
        [Key, Column(Order = 1)]
        public int CategoryId  { get; set; }

        [Key, Column(Order = 2)]
        public int  SupportedCultureId { get; set; }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }

        public virtual Category Category { get; set; }

        public virtual SupportedCulture SupportedCulture { get; set; }

    }
}