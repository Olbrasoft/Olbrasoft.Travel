﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class User : CreationInfo
    {

        [Required]
        [StringLength(255)]
        public string UserName { get; set; }
        

        #region Child Properties

        public virtual ICollection<Continent> CreatedContinents { get; set; }

        public virtual ICollection<Language> CreatedLanguages { get; set; }

        public virtual ICollection<LocalizedContinent> CreatedLocalizedContinents { get; set; }

        public virtual ICollection<Country> CreatedCountries { get; set; }

        public virtual ICollection<LocalizedCountry> CreatedLocalizedCountries { get; set; }
        
        public virtual ICollection<City> CreatedCities { get; set; }
        
        public virtual ICollection<LocalizedCity> CreatedLocalizedCities { get; set; }
        
        public virtual ICollection<Neighborhood> CreatedNeighborhoods { get; set; }
        
        public virtual ICollection<LocalizedNeighborhood> CreatedLocalizedNeighborhoods { get; set; }
        
        public virtual ICollection<TypeOfRegion> CreatedTypesOfRegions { get; set; }
        
        public virtual ICollection<SubClass> CreatedSubClasses { get; set; }

        public virtual ICollection<Region> CreatedRegions { get; set; }

        public virtual ICollection<LocalizedRegion> CreatedLocalizedRegions { get; set; }
        
        public virtual ICollection<RegionToRegion> CreatedRegionsToRegions { get; set; }

        public virtual ICollection<PointOfInterest> CreatedPointsOfInterest { get; set; }

        public virtual ICollection<LocalizedPointOfInterest> CreatedLocalizedPointsOfInterest { get; set; }

        /// <summary>
        /// Created States or Provinces
        /// </summary>
        public virtual ICollection<State> States { get; set; }

        /// <summary>
        /// Created localized States or Provinces
        /// </summary>
        public virtual ICollection<LocalizedState> LocalizedStates { get; set; }



        //public virtual ICollection<PointOfInterestToPointOfInterest> CreatedPointsOfInterestToPointsOfInterest { get; set; }

        //public virtual ICollection<PointOfInterestToRegion> CreatedPointsOfInterestToRegions { get; set; }


        //public virtual ICollection<PointOfInterestToSubClass> CreatedPointsOfInterestToSubClasses { get; set; }

        //public virtual ICollection<Airport> CreatedAirports { get; set; }
        #endregion
    }
}
