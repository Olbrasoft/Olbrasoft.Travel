﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class Country : BaseGeo
    {
        //todo change https://en.wikipedia.org/wiki/ISO_3166-1
        
        [Required]
        [StringLength(2)]
        public string Code { get; set; }

        public int ContinentId { get; set; }

        public Continent Continent { get; set; }

        public virtual User Creator { get; set; }

      //  public virtual ICollection<LocalizedCountry> LocalizedCountries { get; set; }
    }
}
