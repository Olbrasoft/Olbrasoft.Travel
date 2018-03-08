using System;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class Country : TravelEntity
    {
        //todo change https://en.wikipedia.org/wiki/ISO_3166-1

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(2)]
        public string Code { get; set; }

        public int ContinentId { get; set; }

        public int EanRegionId { get; set; }

        public int CreatorId { get; set; }

        public DateTime DateAndTimeOfCreation { get; set; }

        public Continent Continent { get; set; }

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
