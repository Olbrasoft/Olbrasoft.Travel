using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class BaseLocalizedRegionWithLongName : BaseLocalizedRegion
    {
        [StringLength(510)]
        public string LongName { get; set; }
    }
}