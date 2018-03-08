using System;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class BaseRegion : IKeyId
    {
        [Key]
        public int Id { get; set; }

        public int TypeOfRegionId { get; set; }

        public int? SubClassId { get; set; }

        public long EanRegionId { get; set; } = long.MinValue;

        public int CreatorId { get; set; }

        public DateTime DateAndTimeOfCreation { get; set; }
    }
}