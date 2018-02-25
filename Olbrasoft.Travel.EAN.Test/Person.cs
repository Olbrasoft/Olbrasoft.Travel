using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.EAN.Test
{
    public class Person
    {
        [Key]        
        public int Id { get; set; }
        [Required]
        [StringLength(10)]
        public string Name { get; set; }

        public string SurName { get; set; }

    }
}