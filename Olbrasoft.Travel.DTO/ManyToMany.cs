using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Olbrasoft.Travel.DTO
{
    public class ManyToMany : CreatorInfo
    {
        [Key]
        [Column(Order = 2)]
        public int ToId { get; set; }
    }
}