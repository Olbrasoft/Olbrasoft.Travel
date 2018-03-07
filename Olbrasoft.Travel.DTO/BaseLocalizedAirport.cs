using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Olbrasoft.Travel.DTO
{
    public class BaseLocalizedAirport
    {
        [Key, Column(Order = 1)]
        public int AirportId { get; set; }

        [Key, Column(Order = 2)]
        public int LanguageId { get; set; }
        
        public int CreatorId { get; set; }

        public DateTime DateAndTimeOfCreation { get; set; }
    }
}
