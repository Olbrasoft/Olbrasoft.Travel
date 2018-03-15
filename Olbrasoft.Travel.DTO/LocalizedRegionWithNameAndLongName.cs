using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class LocalizedRegionWithNameAndLongName : BaseLocalizedRegion
    {
      

        //added for the correct order of the columns in the Name, LongName table. 
        [Required]
        [StringLength(255)]
        public new string Name { get; set; }

        [StringLength(510)]
        public string LongName { get; set; }

      
    }
}