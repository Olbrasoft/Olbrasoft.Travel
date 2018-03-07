using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olbrasoft.Travel.EAN.DTO.Geography
{
   public class CityNeighborhood
    {
        // ReSharper disable once InconsistentNaming
        [Key]
        public long RegionID { get; set; }
        [Required]
        [StringLength(255)]
        public string RegionName { get; set; }
        [Required]
        public string Coordinates { get; set; }
    }
}
