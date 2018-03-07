using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class LocalizedAirport : BaseLocalizedAirport, ILocalized
    {
        [Required]
        [StringLength(70)]
        public string Name { get; set; }
        
        public User Creator { get; set; }

        public Language Language { get; set; }
    }
}