using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olbrasoft.Travel.DTO
{
    public class LocalizedPointOfInterest:ILocalized
    {
        [Key, Column(Order = 1)]
        public int PointOfInterestId { get; set; }

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

        public virtual PointOfInterest PointOfInterest { get; set; }

        public virtual Language Language { get; set; }
    }
}
