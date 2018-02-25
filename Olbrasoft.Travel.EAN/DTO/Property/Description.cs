using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.EAN.DTO.Property
{
    /// <summary>
    /// https://support.ean.com/hc/en-us/articles/115005784325
    /// File Name: PropertyDescriptionList.txt
    /// Zip File Name: https://www.ian.com/affiliatecenter/include/V2/PropertyDescriptionList.zip
    /// This file holds the English language long general description of the properties in the EAN system.
    /// This is base class for DescriptionMultiLanguage, Amenities,  AmenitiesMultiLanguage, BusinessAmenities,
    /// BusinessAmenitiesMultiLanguage, Location, LocationMultiLanguage, NationalRatings, NationalRatingsMultiLanguage,
    /// Renovations, RenovationsMultiLanguage, Rooms, RoomsMultiLanguage, AreaAttractions, AreaAttractionsMultiLanguage,
    /// DiningDescription, DiningDescriptionMultiLanguage, SpaDescription, WhatToExpect, WhatToExpectMultiLanguage
    /// </summary>
    public class Description
    {
        // ReSharper disable once InconsistentNaming
        [Key]
        public int EANHotelID { get; set; }

        [Required]
        [StringLength(5)]
        public string LanguageCode { get; set; }

        [Required]
        public string PropertyDescription { get; set; }
           
    }
}
