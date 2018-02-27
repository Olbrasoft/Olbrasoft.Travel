using System;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class TaskOfImport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        public int CurrentImportId { get; set; }

        public int Progress { get; set; }

        public DateTime DateAndTimeOfCreation { get; set; }

        public virtual Import CurrentImport { get; set; }

    }
}
