using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class LocalizedRegionWithNameAndLongName : BaseLocalizedRegion
    {
        [StringLength(510)]
        public string LongName { get; set; }
    }
}