﻿using System.Collections.Generic;

namespace Olbrasoft.Travel.DTO
{
    public class Continent : Geo
    {
       // public User Creator { get; set; }

        public virtual ICollection<LocalizedContinent> LocalizedContinents { get; set; }
        public virtual ICollection<Country> Countries { get; set; }
    }
}
