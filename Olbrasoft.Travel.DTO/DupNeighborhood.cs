using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olbrasoft.Travel.DTO
{
   public class DupNeighborhood
    {
        [Key]
        public long RegionId { get; set; }

        [Required]
        [StringLength(255)]
        public string RegionName { get; set; }

        public DbGeography Coordinates { get; set; }
    }



    public class DupCity
    {
        [Key]
        public long RegionId { get; set; }

        [Required]
        [StringLength(255)]
        public string RegionName { get; set; }

        public DbGeography Coordinates { get; set; }
    }


    public class DupPoint
    {
        [Key]
        public long RegionId { get; set; }

        [Required]
        [StringLength(255)]
        public string RegionName { get; set; }

        public DbGeography Coordinates { get; set; }
    }

}
