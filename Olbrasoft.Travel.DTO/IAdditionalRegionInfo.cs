using System.Collections.Generic;

namespace Olbrasoft.Travel.DTO
{
    public interface IAdditionalRegionInfo
    {
        string Code { get; set; }
        Region Region { get; set; }
        ICollection<Accommodation> Accommodations { get; set; }
    }
}