using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.DataTransferObject.Geography
{

    public class ParentContinet : ParentRegion
    {
    }

    /// <summary>
    /// https://support.ean.com/hc/en-us/articles/115005784405-V3-Database-Files-Geography-Data
    /// File Name: ParentRegionList.txt
    /// Zip File Name: https://www.ian.com/affiliatecenter/include/V2/new/ParentRegionList.zip 
    /// This file is the main list of region data.
    /// It can be used to hierarchically search regions from city all the way up to the continent using the ParentRegion data.
    /// </summary>
    public class ParentRegion
    {
        // ReSharper disable once InconsistentNaming
        [Key]
        public long RegionID { get; set; }

        [Required]
        [StringLength(50)]
        public string RegionType { get; set; }

        /// <summary>
        /// (no longer applicable; returns blank space)
        /// </summary>
        [StringLength(3)]
        public string RelativeSignificance { get; set; }

        [StringLength(50)]
        public string SubClass { get; set; }

        [Required]
        [StringLength(255)]
        public string RegionName { get; set; }

        [StringLength(510)]
        public string RegionNameLong { get; set; }

        // ReSharper disable once InconsistentNaming
        public long ParentRegionID { get; set; }

        [StringLength(50)]
        public string ParentRegionType { get; set; }

        [StringLength(255)]
        public string ParentRegionName { get; set; }

        [StringLength(510)]
        public string ParentRegionNameLong { get; set; }

    }
}
