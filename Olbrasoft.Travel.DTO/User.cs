using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olbrasoft.Travel.DTO
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string UserName { get; set; }

        public DateTime DateAndTimeOfCreation { get; set; }

        public virtual ICollection<TypeOfRegion> CreatedTypesOfRegions { get; set; }

        public virtual ICollection<SubClass> CreatedSubClasses { get; set; }

        public virtual ICollection<Region> CreatedRegions { get; set; }

        public virtual ICollection<PointOfInterest> CreatedPointsOfInterest { get; set; }

        public virtual ICollection<Language> CreatedLanguages { get; set; }

        public virtual ICollection<LocalizedRegion> LocalizedRegions { get; set; }

        public virtual ICollection<LocalizedPointOfInterest> LocalizedPointsOfInterest { get; set; }

    }
}
