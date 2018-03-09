using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class LocalizedContinent :Localized
    {
        
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(510)]
        public string LongName { get; set; }
        
        public User Creator { get; set; }
        
        public virtual Continent Continent { get; set; }

        public virtual Language Language { get; set; }
    }
}