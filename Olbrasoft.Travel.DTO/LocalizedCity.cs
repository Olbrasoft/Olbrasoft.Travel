using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class LocalizedCity : BaseLocalized
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public virtual City City { get; set; }

        public User Creator { get; set; }

        public virtual Language Language { get; set; }

    }
}