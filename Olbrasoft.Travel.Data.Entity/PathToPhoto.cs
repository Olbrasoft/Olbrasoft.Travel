using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.Data.Entity
{
    public class PathToPhoto : CreatorInfo
    {
        [Required]
        [StringLength(300)]
        public string Path { get; set; }

       public virtual ICollection<PhotoOfAccommodation> PhotosOfAccommodations { get; set; }
    }
    
}
