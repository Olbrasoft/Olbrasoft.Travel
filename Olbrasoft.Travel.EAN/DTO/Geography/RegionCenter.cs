using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.EAN.DTO.Geography
{
    /// <summary>
    /// https://support.ean.com/hc/en-us/articles/115005784405-V3-Database-Files-Geography-Data
    /// File Name: RegionCenterCoordinatesList.txt
    /// Zip File Name: https://www.ian.com/affiliatecenter/include/V2/new/RegionCenterCoordinatesList.zip 
    /// This file lists the center point of high level regions, multi-city, city and neighborhood regions.
    /// </summary>
    public class RegionCenter : IHaveRegionIdRegionName
    {
        // ReSharper disable once InconsistentNaming
        [Key]
        public long RegionID { get; set; }

        [Required]
        [StringLength(255)]
        public string RegionName { get; set; }

        [Range(typeof(double), "-90", "90")]
        public double CenterLatitude { get; set; }

        [Range(typeof(double), "-180", "180")]
        public double CenterLongitude { get; set; }

    }
}
