using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public interface IKeyId
    {
         [Key]
         int Id { get; set; }
    }

}
