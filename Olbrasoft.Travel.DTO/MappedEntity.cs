using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class MappedEntity:TravelEntity
    {
        [Key]
        public int Id { get; set; }
        public int? EanId { get; set; }
    }
}