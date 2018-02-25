using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.EAN.DTO.Geography
{
    /// <summary>
    /// https://support.ean.com/hc/en-us/articles/115005784405-V3-Database-Files-Geography-Data
    /// File Name: CountryList.txt
    /// Zip File Name: https://www.ian.com/affiliatecenter/include/V2/new/CountryList.zip 
    /// This file contains a list of valid countries around the world.
    /// </summary>
    public class Country
    {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// The CountryID is a link back to the region parent table.
        /// </summary>
        [Key]
        public long CountryID { get; set; }

        [Required]
        [StringLength(5)]
        public string LanguageCode { get; set; }

        [Required]
        [StringLength(256)]
        public string CountryName { get; set; }

        /// <summary>
        /// The CountryID is a link back to the region parent table.
        /// </summary>
        [Required]
        [StringLength(2)]
        public string CountryCode { get; set; }

        /// <summary>
        /// Transliteration is the "clean" version of the country name.
        /// </summary>
        [StringLength(256)]
        public string
        Transliteration { get; set; }

        
        // ReSharper disable once InconsistentNaming
        /// <summary>
        ///  The ContinentID is also a link to the continent name in the region parent table.
        /// </summary>
        public long ContinentID { get; set; }
    }
}
