using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class User : CreationInfo
    {

        [Required]
        [StringLength(255)]
        public string UserName { get; set; }


        #region Child Properties

        public virtual ICollection<TypeOfRegion> CreatedTypesOfRegions { get; set; }
        
        /// <summary>
        /// Regions created by the User.
        /// </summary>
        public virtual ICollection<Region> Regions { get; set; }

        /// <summary>
        ///  SubClasses created by the User.
        /// </summary>
        public virtual ICollection<SubClass> SubClasses { get; set; }

        /// <summary>
        /// Languages created by the User.
        /// </summary>
        public virtual ICollection<Language> Languages { get; set; }


        /// <summary>
        /// Linking Regions to (TypesOfRegions and SubClasses)
        /// Example Praha is Kreis and City
        /// </summary>
        public virtual ICollection<RegionToType> RegionsToTypes { get; set; }


        /// <summary>
        /// LocalizedRegions created by the User.
        /// </summary>
        public virtual ICollection<LocalizedRegion> LocalizedRegions { get; set; }

        ///// <summary>
        ///// Linking Regions and SubClasses created by the User.
        ///// </summary>
        //public virtual ICollection<RegionToSubClass> RegionsToSubClasses { get; set; }


        /// <summary>
        /// Linking Regions To Regions created by the User.
        /// Eaxample Contry to Continent or City to Country.
        /// </summary>
        public virtual ICollection<RegionToRegion> RegionsToRegions { get; set; }

        /// <summary>
        /// Countries created by the User.
        /// </summary>
        public virtual ICollection<Country> Countries { get; set; }


        /// <summary>
        /// Airports created by the User.
        /// </summary>
        public virtual ICollection<Airport> Airports { get; set; }


        //public virtual ICollection<Continent> CreatedContinents { get; set; }


        //public virtual ICollection<LocalizedContinent> CreatedLocalizedContinents { get; set; }



        //public virtual ICollection<LocalizedCountry> CreatedLocalizedCountries { get; set; }

        //public virtual ICollection<City> CreatedCities { get; set; }

        //public virtual ICollection<LocalizedCity> CreatedLocalizedCities { get; set; }

        //public virtual ICollection<Neighborhood> CreatedNeighborhoods { get; set; }

        //public virtual ICollection<LocalizedNeighborhood> CreatedLocalizedNeighborhoods { get; set; }





        //public virtual ICollection<PointOfInterest> CreatedPointsOfInterest { get; set; }

        //public virtual ICollection<LocalizedPointOfInterest> CreatedLocalizedPointsOfInterest { get; set; }

        ///// <summary>
        ///// Created States or Provinces.
        ///// </summary>
        //public virtual ICollection<State> States { get; set; }

        ///// <summary>
        ///// Created localized States or Provinces.
        ///// </summary>
        //public virtual ICollection<LocalizedState> LocalizedStates { get; set; }

        ///// <summary>
        ///// Linking Cities and Countries created by the User.
        ///// </summary>
        //public virtual ICollection<CityToCountry> CitiesToCountries { get; set; }

        ///// <summary>
        ///// Linking Cities and SubClasses created by the User.
        ///// </summary>
        //public virtual ICollection<CityToSubClass> CitiesToSubClasses { get; set; }


        ///// <summary>
        ///// Linking Cities and (Multi-Region (within a country)) or (Multi-City (Vicinity)) created by the User.
        ///// </summary>
        //public virtual ICollection<CityToRegion> CitiesToRegions { get; set; }

        //public virtual ICollection<PointOfInterestToPointOfInterest> CreatedPointsOfInterestToPointsOfInterest { get; set; }

        //public virtual ICollection<PointOfInterestToRegion> CreatedPointsOfInterestToRegions { get; set; }

        //public virtual ICollection<PointOfInterestToSubClass> CreatedPointsOfInterestToSubClasses { get; set; }

        //public virtual ICollection<Airport> CreatedAirports { get; set; }

        #endregion
    }
}
