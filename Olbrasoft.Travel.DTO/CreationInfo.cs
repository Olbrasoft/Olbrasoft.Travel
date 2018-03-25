using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Olbrasoft.Travel.DTO
{
    public class CreationInfo : IKeyId
    {
        [Key, Column(Order = 1)]
        public int Id { get; set; }
        public DateTime DateAndTimeOfCreation { get; set; }
    }
}