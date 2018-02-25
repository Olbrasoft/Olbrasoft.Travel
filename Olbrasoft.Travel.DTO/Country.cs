using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olbrasoft.Travel.DTO
{
    public class Country:TravelEntity
    {
        [Key]
        public int RegionId { get; set; }

        [Required]
        [StringLength(2)]
        public string Code { get; set; }

        //[Key]
        //public long CountryID { get; set; }
        
        //public string LanguageCode { get;set; }
        //    //nvarchar(5)
        //public string CountryName { get; set; }
        //    //nvarchar(256)
        //public string CountryCode { get; set; }
        ////nvarchar(2)

        //public string Transliteration { get; set; }
        ////nvarchar(256)

        //public long ContinentID { get; set; }
    }
}
