using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class LocalizedRegion : LocalizedRegionWithNameAndLongName
    {
       
        public User Creator { get; set; }

        public virtual Region Region { get; set; }

        public virtual Language Language { get; set; }
    }
}
