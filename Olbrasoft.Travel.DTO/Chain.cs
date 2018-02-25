using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public partial class Chain:MappedEntity
    {
       
        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        public virtual ICollection<Accommodation> Accommodations { get; set; }
    }
}
