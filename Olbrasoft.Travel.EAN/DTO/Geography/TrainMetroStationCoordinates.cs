using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.EAN.DTO.Geography
{
    /// <summary>
    /// https://support.ean.com/hc/en-us/articles/115005784405-V3-Database-Files-Geography-Data
    /// File Name: TrainMetroStationCoordinatesList.txt
    /// Zip File Name: https://www.ian.com/affiliatecenter/include/V2/new/TrainMetroStationCoordinatesList.zip 
    /// This file contains a list of all train stations and metro stations by latitude and longitude.
    /// </summary>
    public class TrainMetroStationCoordinates : IHaveRegionIdRegionName, IHaveRegionIdLatitudeLongitude
    {
        [Key]
        // ReSharper disable once InconsistentNaming
        public long RegionID { get; set; }

        [StringLength(255)]
        public string RegionName { get; set; }

        [StringLength(50)]
        public string RegionType { get; set; }

        [Range(typeof(double), "-90", "90")]
        public double Latitude { get; set; }

        [Range(typeof(double), "-180", "180")]
        public double Longitude { get; set; }

    }
}
