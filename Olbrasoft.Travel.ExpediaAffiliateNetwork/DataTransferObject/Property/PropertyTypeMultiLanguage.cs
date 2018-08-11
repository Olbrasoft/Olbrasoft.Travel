using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.DataTransferObject.Property
{
    /// <summary>
    /// File Name: PropertyTypeList_xx_XX.txt
    /// Zip File Name: https://www.ian.com/affiliatecenter/include/V2/PropertyTypeList_xx_XX.zip
    /// This file holds all non-English language translations for the records located in the PropertyTypeList file.
    /// Use the URL format provided above, substituting the _xx_XX suffix with your desired:
    /// https://support.ean.com/hc/en-us/articles/115005813145
    /// </summary>
    public class PropertyTypeMultiLanguage
    {
        public int PropertyCategory { get; set; }

        [Required]
        [StringLength(5)]
        public  string LanguageCode { get; set; }

        [Required]
        [StringLength(256)]
        public string PropertyCategoryDesc { get; set; }
    }
}
