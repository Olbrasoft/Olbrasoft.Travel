using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class TypeOfDescription :BaseType
    {
      
        public virtual ICollection<Description> Descriptions { get; set; }
    }
}