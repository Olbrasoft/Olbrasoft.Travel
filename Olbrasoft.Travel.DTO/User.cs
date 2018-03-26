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

        /// <summary>
        /// TypesOfAccommodations created by the User.
        /// </summary>
        public virtual ICollection<TypeOfAccommodation> TypesOfAccommodations { get; set; }

        /// <summary>
        /// LocalizedTypesOfAccommodations created by the User.
        /// </summary>
        public virtual ICollection<LocalizedTypeOfAccommodation> LocalizedTypesOfAccommodations { get; set; }

        /// <summary>
        /// Chains created by the User.
        /// </summary>
        public virtual ICollection<Chain> Chains { get; set; }

        /// <summary>
        /// Accommodations created by the User.
        /// </summary>
        public virtual ICollection<Accommodation> Accommodations { get; set; }

        /// <summary>
        /// LocalizedAccommodations created by the User.
        /// </summary>
        public virtual ICollection<LocalizedAccommodation> LocalizedAccommodations { get; set; }

        /// <summary>
        /// Types of Descriptions created by the User.
        /// </summary>
        public virtual ICollection<TypeOfDescription> TypesOfDescriptions { get; set; }

        /// <summary>
        /// Descriptions Of Accommodations is Loacalized and Categorized 
        /// </summary>
        public virtual ICollection<Description> Descriptions { get; set; }

        #endregion
    }
}
