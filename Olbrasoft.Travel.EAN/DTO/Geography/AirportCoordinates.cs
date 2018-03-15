using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.EAN.DTO.Geography
{
    /// <summary>
    /// https://support.ean.com/hc/en-us/articles/115005784405-V3-Database-Files-Geography-Data
    /// File Name: AirportCoordinatesList.txt
    /// Zip File Name: https://www.ian.com/affiliatecenter/include/V2/new/AirportCoordinatesList.zip 
    /// This file contains a list of airports and their matching RegionID(MainCityID). 
    /// AirportCode is the three letter airport abbreviation.
    /// Latitude and Longitude show the geographic coordinates of the airport.
   /// </summary>
    public class AirportCoordinates
    {

        // ReSharper disable once InconsistentNaming
        [Key]
        public long AirportID { get; set; }

        [Required]
        [StringLength(3)]
        public string AirportCode { get; set; }

        [Required]
        [StringLength(70)]
        public string AirportName { get; set; }

        [Range(typeof(double), "-90", "90")]
        public double Latitude { get; set; }

        [Range(typeof(double), "-180", "180")]
        public double Longitude { get; set; }
        
        // ReSharper disable once InconsistentNaming
        /// <summary>
        ///  MainCityID is the RegionID of the major city or multi-city the airport serves.
        /// </summary>
        public long? MainCityID { get; set; }

        [StringLength(2)]
        public string CountryCode { get; set; }
    }
}
