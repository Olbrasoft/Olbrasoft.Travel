using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.EAN.DTO.Geography
{
    /// <summary>
    /// https://support.ean.com/hc/en-us/articles/115005784405-V3-Database-Files-Geography-Data
    /// File Name: PointsOfInterestCoordinatesList.txt
    /// Zip File Name: https://www.ian.com/affiliatecenter/include/V2/new/PointsOfInterestCoordinatesList.zip 
    /// This file is a listing of all points of interest by latitude and longitude.
    /// The SubClassification shows what type of POI it is.
    /// </summary>
    public class PointOfInterestCoordinates :IHaveRegionIdRegionNameRegionNameLong
    {
        // ReSharper disable once InconsistentNaming
        [Key]
        public long RegionID { get; set; }

        [Required]
        [StringLength(255)]
        public string RegionName { get; set; }

        [StringLength(510)]
        public string RegionNameLong { get; set; }

        [Range(typeof(double), "-90", "90")]
        public double Latitude { get; set; }

        [Range(typeof(double), "-180", "180")]
        public double Longitude { get; set; }

        [StringLength(20)]
        public string SubClassification { get; set; }
    }
}
