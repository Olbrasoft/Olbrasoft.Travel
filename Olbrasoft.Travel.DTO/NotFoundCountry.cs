using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olbrasoft.Travel.DTO
{
    public class NotFoundCountry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(2)]
        public string Code { get; set; }
    }



}
