using System;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class BaseAirport
    {
        [Key]
        public int Id { get; set; }

        public long EanAirportId { get; set; } = long.MinValue;

        public int CreatorId { get; set; }

        public DateTime DateAndTimeOfCreation { get; set; }
    }
}
