using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Olbrasoft.Travel.DTO
{


    public class LocalizedAccommodation :ILocalized
    {
        [Key, Column(Order = 1)]
        public int AccommodationId { get; set; }
        [Key, Column(Order = 2)]
        public int SupportedCultureId { get; set; }
        [Required]
        [StringLength(70)]
        public string Name { get; set; }
        [StringLength(80)]
        public string Location { get; set; }
        [StringLength(10)]
        public string CheckInTime { get; set; }
        [StringLength(10)]
        public string CheckOutTime { get; set; }
        public virtual Accommodation Accommodation { get; set; }
        public virtual SupportedCulture SupportedCulture { get; set; }

    }
}
