using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.EAN.DTO.Geography
{
    /// <summary>
    /// https://support.ean.com/hc/en-us/articles/115005784405-V3-Database-Files-Geography-Data
    /// File Name: CityCoordinatesList.txt
    /// Zip File Name: https://www.ian.com/affiliatecenter/include/V2/new/CityCoordinatesList.zip 
    /// This file contains a list of cities and their matching RegionID.
    /// The Coordinates field is a colon delimited list of Latitude; Longitude values.
    /// </summary>
    public class City : CityNeighborhood
    {

    }
}
