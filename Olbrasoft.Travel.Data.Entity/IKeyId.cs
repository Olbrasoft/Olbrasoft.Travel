using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.Data.Entity
{
    public interface IKeyId
    {
         [Key]
         int Id { get; set; }
    }

}
