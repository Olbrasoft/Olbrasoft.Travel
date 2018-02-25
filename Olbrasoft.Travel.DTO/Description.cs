using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Olbrasoft.Travel.DTO
{
    public class Description: ILocalized
    {
        [Key]
        [Column(Order = 1)]
        public int AccommodationId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int TypeOfDescriptionId { get; set; }

        [Key]
        [Column(Order = 3)]
        public int SupportedCultureId { get; set; }

        [Required]
        public string Text { get; set; }

        public virtual Accommodation Accommodation { get; set; }

        public virtual TypeOfDescription TypeOfDescription { get; set; }

        public virtual SupportedCulture SupportedCulture { get; set; }
    }
}
