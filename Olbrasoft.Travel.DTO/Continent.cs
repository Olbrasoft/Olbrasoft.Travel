﻿using System.Collections.Generic;

namespace Olbrasoft.Travel.DTO
{
    public class Continent : BaseRegion
    {
        public virtual ICollection<LocalizedContinent> LocalizedContinents { get; set; }
    }
}