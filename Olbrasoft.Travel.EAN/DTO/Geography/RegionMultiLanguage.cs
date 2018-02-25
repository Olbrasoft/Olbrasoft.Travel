using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.EAN.DTO.Geography
{
    /// <summary>
    /// https://support.ean.com/hc/en-us/articles/115005784405-V3-Database-Files-Geography-Data
    /// File Name: RegionList_xx_XX.txt
    /// Zip File Name:https://www.ian.com/affiliatecenter/include/V2/new/RegionList_xx_XX.zip 
    /// This file holds all language translations for each region including English language.
    /// Use the URL format provided above, substituting the _xx_XX suffix with your desired:
    /// https://support.ean.com/hc/en-us/articles/115005813145
    /// </summary>
    public class RegionMultiLanguage
    {
        [Key]
        // ReSharper disable once InconsistentNaming
        public long RegionID { get; set; }

        [StringLength(5)]
        public string LanguageCode { get; set; }

        [Required]
        [StringLength(255)]
        public string RegionName { get; set; }

        [StringLength(510)]
        public string RegionNameLong { get; set; }
    }
}
