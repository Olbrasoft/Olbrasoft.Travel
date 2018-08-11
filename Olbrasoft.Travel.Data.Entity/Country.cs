﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.Data.Entity
{
    public class Country : CreatorInfo , IAdditionalRegionInfo
    {
        //todo change https://en.wikipedia.org/wiki/ISO_3166-1

        [Required]
        [StringLength(2)]
        public string Code { get; set; }

        public virtual Region Region { get; set; }

        public virtual ICollection<Accommodation> Accommodations { get; set; }

    }
    

}
