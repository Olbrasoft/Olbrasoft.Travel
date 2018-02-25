using System.Collections.Generic;

namespace Olbrasoft.Travel.DTO
{
    public class SubClassOfRegion : BaseType
    {
        public virtual ICollection<Region> Regions { get; set; }
    }
}
