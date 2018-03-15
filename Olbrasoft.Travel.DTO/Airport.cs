using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;

namespace Olbrasoft.Travel.DTO
{
    public class Airport : Creator, IMapToPartners<long>
    {
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// https://en.wikipedia.org/wiki/List_of_airports_by_IATA_code:_A
        /// </summary>
        [Required]
        [StringLength(3)]
        public string Code { get; set; }
        
        [Required]
        public DbGeography Coordinates { get; set; }

        public long EanId { get; set; } = long.MinValue;
        
        public User Creator { get; set; }
    }
}
