namespace Olbrasoft.Travel.EAN.DTO.Geography
{
    /// <summary>
    /// https://support.ean.com/hc/en-us/articles/115005784405-V3-Database-Files-Geography-Data
    /// File Name: NeighborhoodCoordinatesList.txt
    /// Zip File Name: https://www.ian.com/affiliatecenter/include/V2/new/NeighborhoodCoordinatesList.zip 
    /// This file contains a list of neighborhood and their matching RegionID.
    /// The Coordinates field is a colon delimited list of Latitude; Longitude values.
    /// </summary>
    public class NeighborhoodCoordinates : CityNeighborhood
    {
        //// ReSharper disable once InconsistentNaming
        //[Key]
        //public long RegionID { get; set; }

        //[Required]
        //[StringLength(255)]
        //public string RegionName { get; set; }

        //[Required]
        //public string Coordinates { get; set; }
    }

}
