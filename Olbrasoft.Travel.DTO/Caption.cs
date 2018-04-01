using System.Collections.Generic;

namespace Olbrasoft.Travel.DTO
{
    public class Caption : CreatorInfo
    {
       public virtual ICollection<LocalizedCaption> LocalizedCaptions { get; set; }

       public virtual ICollection<PhotoOfAccommodation> PhotosOfAccommodations { get; set; }
    }
    
}
