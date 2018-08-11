﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.Data.Entity
{
    public class FileExtension : CreatorInfo
    {
        [Required]
        [StringLength(50)]
        public string Extension { get; set; }

        public virtual ICollection<PhotoOfAccommodation> PhotosOfAccommodations { get; set; }
    }
}
