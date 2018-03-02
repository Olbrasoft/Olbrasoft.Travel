using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olbrasoft.Travel.DTO
{
    public class LogOfImport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Log { get; set; }

        public DateTime DateAndTimeOfCreation { get; set; }
    }
}
