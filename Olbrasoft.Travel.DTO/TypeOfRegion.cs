using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class TypeOfRegion : BaseType
    {
        public virtual ICollection<Region> Regions { get; set; }
    }
}
