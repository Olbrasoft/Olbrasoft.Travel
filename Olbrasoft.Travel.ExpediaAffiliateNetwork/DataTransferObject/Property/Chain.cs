using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.DataTransferObject.Property
{
    /// <summary>
    /// https://support.ean.com/hc/en-us/articles/115005784325
    /// File Name: ChainList.txt
    /// Zip File Name: https://www.ian.com/affiliatecenter/include/V2/ChainList.zip
    /// This is a listing of all chain names.
    /// It can be used in conjunction with the ActivePropertyList file to link the ChainCodeID to a chain name.
    /// </summary>
    public class Chain
    {
        // ReSharper disable once InconsistentNaming
        [Key]
        public int ChainCodeID { get; set; }

        [Required]
        [StringLength(30)]
        public  string ChainName { get; set; }
    }
}
