using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.EAN.Import.Development
{
    public class DevelopmentTask
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

    }
}