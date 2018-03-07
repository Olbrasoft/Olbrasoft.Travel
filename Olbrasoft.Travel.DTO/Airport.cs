using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olbrasoft.Travel.DTO
{
    public class Airport : BaseAirport
    {
        /// <summary>
        /// https://en.wikipedia.org/wiki/List_of_airports_by_IATA_code:_A
        /// </summary>
        [Required]
        [StringLength(3)]
        public string Code { get; set; }

        [Required]
        public DbGeography Coordinates { get; set; }

        public User Creator { get; set; }
    }
}
