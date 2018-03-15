using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class LogOfImport : CreationInfo
    {
        [Required]
        [StringLength(255)]
        public string Log { get; set; }

    }
}
