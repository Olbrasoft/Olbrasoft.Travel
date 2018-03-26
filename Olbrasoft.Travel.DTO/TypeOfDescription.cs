using System.Collections.Generic;

namespace Olbrasoft.Travel.DTO
{
    public class TypeOfDescription : BaseName
    {
        public virtual ICollection<Description> Descriptions { get; set; }
    }
}