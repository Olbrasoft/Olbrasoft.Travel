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
        //todo change https://en.wikipedia.org/wiki/ISO_3166-1

        [Key]
        public int RegionId { get; set; }

        [Required]
        [StringLength(2)]
        public string Code { get; set; }

        public int CreatorId { get; set; }

        public DateTime DateAndTimeOfCreation { get; set; }

        public User Creator { get; set; }



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
