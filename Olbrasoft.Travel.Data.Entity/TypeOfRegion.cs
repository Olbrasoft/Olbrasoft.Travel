﻿using System.Collections.Generic;

namespace Olbrasoft.Travel.Data.Entity
{
    public class TypeOfRegion : BaseName
    {
        public ICollection<RegionToType> RegionsToTypes { get; set; }
        

       //public virtual ICollection<Region> Regions { get; set; }
    }
}
