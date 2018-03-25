using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class Language : CreatorInfo
    {
      
        [StringLength(5)]
        public string EanLanguageCode { get; set; }

        public virtual ICollection<LocalizedRegion> LocalizedRegions { get; set; }
        
        //public virtual ICollection<LocalizedContinent> LocalizedContinents { get; set; }

        //public virtual ICollection<LocalizedCountry> LocalizedCountries { get; set; }
        
        //public virtual ICollection<LocalizedCity> LocalizedCities { get; set; }
        
        //public virtual ICollection<LocalizedNeighborhood> LocalizedNeighborhoods { get; set; }
        
        //public virtual ICollection<LocalizedPointOfInterest> LocalizedPointsOfInterest { get; set; }

        ///// <summary>
        ///// Information about the state or province in the current language
        ///// </summary>
        //public virtual ICollection<LocalizedState> LocalizedStates { get; set; }
 
    }
}
