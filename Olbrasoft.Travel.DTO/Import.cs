using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olbrasoft.Travel.DTO
{
    public sealed class Import
    {
        public Import()
        {
            Tasks = new HashSet<TaskOfImport>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        public int CreatorId { get; set; }

        public DateTime DateAndTimeOfCreation { get; set; }

        [Required]
        public int CurrentStatusId { get; set; }

        public User Creator { get; set; }

        public Status CurrentStatus { get; set; }

        public ICollection<TaskOfImport> Tasks { get; set; }
    }
}
