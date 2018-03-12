using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class LocalizedCity : LocalizedRegionWithNameAndLongName
    {
    
        public virtual City City { get; set; }

        public User Creator { get; set; }

        public virtual Language Language { get; set; }

    }
}