using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Olbrasoft.Travel.EAN.DTO.Property
{
    /// <summary>
    /// https://support.ean.com/hc/en-us/articles/115005784385
    /// filename: AttributeList
    /// Direct download link (being deprecated): https://www.ian.com/affiliatecenter/include/V2/AttributeList.zip 
    /// This is a list of all amenity and short policy data. It is broken down by type. 
    /// The possible types are PropertyAmenity, RoomAmenity and Policy. 
    /// Policy is further broken down by Pet, CheckInOut, Payment and Other policies. 
    /// Each Attribute is mapped to a property in the PropertyAttributeLink file.
    /// </summary>
    public class Attribute
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        // ReSharper disable once InconsistentNaming
        public int AttributeID { get; set; }

        [Required]
        [StringLength(5)]
        public string LanguageCode { get; set; }

        [StringLength(255)]
        public string AttributeDesc { get; set; }

        [StringLength(15)]
        public string Type { get; set; }

        [StringLength(15)]
        public string SubType { get; set; }

    }
}