using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class LocalizedEntityRegion : LocalizedEntity, ILocalized
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(510)]
        public string LongName { get; set; }
        
        public User Creator { get; set; }

        public virtual Region Region { get; set; }

        public virtual Language Language { get; set; }

    }
}
