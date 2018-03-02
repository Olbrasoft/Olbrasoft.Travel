﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Olbrasoft.Travel.DTO
{
    public class LocalizedRegion :ILocalized
    {
        [Key, Column(Order = 1)]
        public int RegionId { get; set; }

        [Key, Column(Order = 2)]
        public int LanguageId { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(510)]
        public string LongName { get; set; }

        public int CreatorId { get; set; }

        public DateTime DateAndTimeOfCreation { get; set; }

        public User Creator { get; set; }

        public virtual Region Region { get; set; }

        public virtual Language Language { get; set; }

    }
}
