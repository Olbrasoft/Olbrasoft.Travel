
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class LocalizedRegion : Localized
    {
        [Required]
        [StringLength(255)]
        public virtual string Name { get; set; }

        [StringLength(510)]
        public string LongName { get; set; }

        public virtual Region Region { get; set; }
    }
}
