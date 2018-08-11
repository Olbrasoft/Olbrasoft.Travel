using System.Collections.Generic;

namespace Olbrasoft.Travel.Data.Entity
{
    public class TypeOfAccommodation : CreatorInfo, IHaveEanId<int>
    {
        public int EanId { get; set; } = int.MinValue;

        public virtual ICollection<LocalizedTypeOfAccommodation> LocalizedTypesOfAccommodations { get; set; }

        public virtual ICollection<Accommodation> Accommodations { get; set; }
    }
}