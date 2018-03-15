using System.Collections.Generic;

namespace Olbrasoft.Travel.DTO
{
    public class Continent : BaseGeo
    {
        public virtual User Creator { get; set; }
        public virtual ICollection<LocalizedContinent> LocalizedContinents { get; set; }
        public virtual ICollection<Country> Countries { get; set; }
    }
}
