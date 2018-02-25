using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Olbrasoft.Travel.EAN.DTO
{
    /// <summary>
    /// https://support.ean.com/hc/en-us/articles/115005784405-V3-Database-Files-Geography-Data
    /// File Name: RegionEANHotelIDMapping.txt
    /// Zip File Name: https://www.ian.com/affiliatecenter/include/V2/new/RegionEANHotelIDMapping.zip 
    /// This file links EAN properties to province/state, high level region, multi-city, city, neighborhood,
    /// point of interest and select country regions by EANHotelID.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class RegionEANHotelIDMapping
    {
        // ReSharper disable once InconsistentNaming
        [Key, Column(Order = 1)]
        public long RegionID { get; set; }

        // ReSharper disable once InconsistentNaming
        [Key, Column(Order = 2)]
        public int EANHotelID { get; set; }
    }
}
