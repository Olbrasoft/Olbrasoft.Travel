using System.Dynamic;

namespace Olbrasoft.Travel.DTO
{
   
    public class LocalizedPointOfInterest :LocalizedRegionWithNameAndLongName
    {
        
        public virtual PointOfInterest PointOfInterest { get; set; }
    }
}
