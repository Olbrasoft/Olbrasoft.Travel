using System.Collections.Generic;

namespace Olbrasoft.Travel.DTO
{
    public class SubTypeOfAttribute : BaseName
    {
        public ICollection<Attribute> Attributes { get; set; }
    }
}