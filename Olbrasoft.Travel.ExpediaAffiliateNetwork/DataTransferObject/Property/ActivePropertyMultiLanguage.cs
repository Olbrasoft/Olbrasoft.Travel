using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.DataTransferObject.Property
{

    /// <summary>
    /// https://support.ean.com/hc/en-us/articles/115005784325
    /// File Name: ActivePropertyList_xx_XX.txt
    /// Zip File Name: https://www.ian.com/affiliatecenter/include/V2/ActivePropertyList_xx_XX.zip
    /// This file holds all non-English language translations for the records located in the ActivePropertyList file.
    /// Use the URL format provided above, substituting the _xx_XX suffix with your desired:
    /// https://support.ean.com/hc/en-us/articles/115005813145
    /// The English language Active Property List, above this entry, is the master list of all of the Expedia-collect properties.
    /// Our multi-language files also contain properties which are Hotel Collect inventory.
    /// These additional hotels are only available for sale by partners who are permitted and configured to sell this type of inventory.
    /// Please use the set of hotels in the English language file as your master set of properties to sell.
    /// </summary>
    public class ActivePropertyMultiLanguage: IToLocalizedAccommodation
    {
        // ReSharper disable once InconsistentNaming
        [Key]
        public int EANHotelID { get; set; }

        [StringLength(5)]
        public string LanguageCode { get; set; }

        [Required]
        [StringLength(70)]
        public string Name { get; set; }

        [StringLength(80)]
        public string Location { get; set; }

        [StringLength(10)]
        public string CheckInTime { get; set; }

        [StringLength(10)]
        public string CheckOutTime { get; set; }
    }
}
