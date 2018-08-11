using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.DataTransferObject.Property
{
    /// <summary>
    /// https://support.ean.com/hc/en-us/articles/115005784325
    /// File Name: ActivePropertyList.txt
    /// Zip File Name: https://www.ian.com/affiliatecenter/include/V2/ActivePropertyList.zip 
    /// This is a list of all active properties in the EAN database with English language data.
    /// A possible value for this field is "Near Times Square".
    /// The chain code can be mapped to the ChainList file to get the chain name.
    /// Use Latitude/Longitude to map properties to your own database of properties when using other content sources.
    /// </summary>
    public class ActiveProperty : IToLocalizedAccommodation
    {
        // ReSharper disable once InconsistentNaming
        [Key]
        public int EANHotelID { get; set; }

        /// <summary>
        /// Properties can sorted by performance via the SequenceNumber field.
        /// The SequenceNumber value ranks properties based on EAN transactional data from the last 30 days, 
        /// with 1 indicating the best-performing hotel and others following in ascending numerical order.
        /// This is the same sort algorithm used for dateless hotel list requests; values are updated daily.
        /// </summary>
        public int SequenceNumber { get; set; }

        [Required]
        [StringLength(70)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Address1 { get; set; }

        [StringLength(50)]
        public string Address2 { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(2)]
        public string StateProvince { get; set; }

        [StringLength(15)]
        public string PostalCode { get; set; }

        [StringLength(2)]
        public string Country { get; set; }

        [Range(typeof(double), "-90", "90")]
        public double Latitude { get; set; }

        [Range(typeof(double), "-180", "180")]
        public double Longitude { get; set; }

        [StringLength(3)]
        public string AirportCode { get; set; }

        public int PropertyCategory { get; set; }

        [StringLength(3)]
        public string PropertyCurrency { get; set; }

        public decimal StarRating { get; set; }

        public int Confidence { get; set; }

        [StringLength(3)]
        public string SupplierType { get; set; }

        /// <summary>
        ///Short description of the location of the property.
        /// </summary>
        [StringLength(80)]
        public string Location { get; set; }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        ///(The chain code can be mapped to the ChainList file to get the chain name.)
        /// </summary>
        [StringLength(5)]
        public string ChainCodeID { get; set; }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// (no longer applicable; returns blank space)
        /// </summary>
        public int RegionID { get; set; }

        /// <summary>
        /// (do not use – no longer maintained)
        /// </summary>
        public double HighRate { get; set; }

        /// <summary>
        /// (do not use – no longer maintained)
        /// </summary>
        public double LowRate { get; set; }

        [StringLength(10)]
        public string CheckInTime { get; set; }

        [StringLength(10)]
        public string CheckOutTime { get; set; }

    }
}
