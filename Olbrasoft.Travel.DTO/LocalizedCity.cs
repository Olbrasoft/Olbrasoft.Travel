using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class LocalizedCity : BaseLocalizedRegion
    {
        public virtual City City { get; set; }

    }
}