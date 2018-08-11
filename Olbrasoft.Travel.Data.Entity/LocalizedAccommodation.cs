using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.Data.Entity
{
    public class LocalizedAccommodation : Localized
    {
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
    }
}
