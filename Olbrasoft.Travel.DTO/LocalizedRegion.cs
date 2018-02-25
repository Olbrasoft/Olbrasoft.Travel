using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Olbrasoft.Travel.DTO
{
    public class LocalizedRegion :TravelEntity, ILocalized
    {
        [Key, Column(Order = 1)]
        public int RegionId { get; set; }
        [Key, Column(Order = 2)]
        public int SupportedCultureId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(510)]
        public string NameLong { get; set; }

        public virtual Region Region { get; set; }
        public virtual SupportedCulture SupportedCulture { get; set; }

    }
}
