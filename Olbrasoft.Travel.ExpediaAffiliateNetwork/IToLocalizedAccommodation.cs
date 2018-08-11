using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork
{
    public interface IToLocalizedAccommodation
    {
        [Key]
        // ReSharper disable once InconsistentNaming
        int EANHotelID { get; set; }
        
        [Required]
        [StringLength(70)]
        string Name { get; set; }

        [StringLength(80)]
        string Location { get; set; }

        string CheckInTime { get; set; }

        [StringLength(10)]
        string CheckOutTime { get; set; }
    }
}
