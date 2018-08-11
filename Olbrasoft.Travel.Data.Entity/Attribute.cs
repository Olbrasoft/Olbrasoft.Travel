using System.Collections.Generic;

namespace Olbrasoft.Travel.Data.Entity
{
    public class Attribute : CreatorInfo, IHaveEanId<int>
    {
        public int TypeOfAttributeId { get; set; }
        public int SubTypeOfAttributeId { get; set; }
        public int EanId { get; set; } = int.MinValue;

        public TypeOfAttribute TypeOfAttribute { get; set; }
        public SubTypeOfAttribute SubTypeOfAttribute { get; set; }
        public ICollection<LocalizedAttribute> LocalizedAttributes { get; set; }
        public virtual ICollection<AccommodationToAttribute> AccommodationsToAttributes { get; set; }

    }
}