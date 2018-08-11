using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.Data.Entity
{
    public class LogOfImport : CreationInfo
    {
        [Required]
        [StringLength(255)]
        public string Log { get; set; }

    }
}
