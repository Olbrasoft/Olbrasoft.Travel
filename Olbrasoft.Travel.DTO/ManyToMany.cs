using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Olbrasoft.Travel.DTO
{
    public class ManyToMany : TravelEntity
    {
        [Key]
        [Column(Order = 1)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 2)]
        public int ToId { get; set; }

        public int CreatorId { get; set; }

        public DateTime DateAndTimeOfCreation { get; set; }
    }
}