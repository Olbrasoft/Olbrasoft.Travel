using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.EAN.DTO.Geography
{
    /// <summary>
    /// https://support.ean.com/hc/en-us/articles/115005784405-V3-Database-Files-Geography-Data
    /// File Name: CountryList_xx_XX.txt
    /// Zip File Name:https://www.ian.com/affiliatecenter/include/V2/new/CountryList_xx_XX.zip 
    /// This file holds all non-English language translations for the records located in the CountryList file.
    /// Use the URL format provided above, substituting the _xx_XX suffix with your desired:
    /// https://support.ean.com/hc/en-us/articles/115005813145
    /// </summary>
    public class CountryMultiLanguage
    {
        // ReSharper disable once InconsistentNaming
        [Key]
        public long CountryID { get; set; }

        [StringLength(5)]
        public string LanguageCode { get; set; }

        [StringLength(256)]
        public string CountryName { get; set; }
    }
}
