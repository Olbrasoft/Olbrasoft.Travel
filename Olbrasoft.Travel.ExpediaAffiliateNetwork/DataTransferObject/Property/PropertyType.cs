using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.DataTransferObject.Property
{
    /// <summary>
    /// https://support.ean.com/hc/en-us/articles/115005784325
    /// File Name: PropertyTypeList.txt
    /// Zip File Name: https://www.ian.com/affiliatecenter/include/V2/PropertyTypeList.zip
    /// This is a mapping list of property types for the given PropertyCategory.
    /// This can be used in conjunction with the ActivePropertyList.
    /// </summary>
    public class PropertyType
    {
        [Key]
        public int PropertyCategory { get; set; }
        
        [Required]
        [StringLength(5)]
        public string LanguageCode { get; set; }

        [Required]
        [StringLength(256)]
        public string
        PropertyCategoryDesc { get; set; }
    }
}
