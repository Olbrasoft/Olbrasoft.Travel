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
        
        public virtual ICollection<Status> CreatedStatuses { get; set; }
        public virtual ICollection<Import> CreatedImports { get; set; }
        
    }
}
