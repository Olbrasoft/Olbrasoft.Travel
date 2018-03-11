using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class LocalizedContinent : BaseLocalizedRegionWithLongName
    {
       
        public User Creator { get; set; }
        
        public virtual Continent Continent { get; set; }

        public virtual Language Language { get; set; }
    }
}