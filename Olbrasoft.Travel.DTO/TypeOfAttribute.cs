using System.Collections.Generic;

namespace Olbrasoft.Travel.DTO
{
    public class TypeOfAttribute : BaseName
    {
        public ICollection<Attribute> Attributes { get; set; }
    }
}
