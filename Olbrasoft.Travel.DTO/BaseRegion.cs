using System;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class BaseRegion : IKeyId
    {
        [Key]
        public int Id { get; set; }

        public long EanRegionId { get; set; } = long.MinValue;

        public int CreatorId { get; set; }

        public DateTime DateAndTimeOfCreation { get; set; }

        public User Creator { get; set; }
    }
}