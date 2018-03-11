using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class LocalizedNeighborhood : BaseLocalized
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public virtual Neighborhood Neighborhood { get; set; }

        public User Creator { get; set; }

        public virtual Language Language { get; set; }
    }
}