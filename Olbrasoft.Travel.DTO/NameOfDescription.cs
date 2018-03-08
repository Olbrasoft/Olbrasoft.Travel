using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.DTO
{
    public class NameOfDescription :BaseName
    {
      
        public virtual ICollection<Description> Descriptions { get; set; }
    }
}