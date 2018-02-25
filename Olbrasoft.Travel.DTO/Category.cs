using System.Collections.Generic;

namespace Olbrasoft.Travel.DTO
{
    public partial class Category : MappedEntity
    {
        public virtual ICollection<Accommodation> Accommodations { get; set; }
    }
}
