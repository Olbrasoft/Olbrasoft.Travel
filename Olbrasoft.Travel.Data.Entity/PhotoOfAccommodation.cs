using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.Data.Entity
{
    public class PhotoOfAccommodation : CreatorInfo
    {
        public int AccommodationId { get; set; }
        
        public int PathToPhotoId { get; set; }

        [StringLength(50)]
        public string FileName { get; set; }

        public int FileExtensionId { get; set; }
      
        public bool IsDefault { get; set; }
        
        public int? CaptionId { get; set; }

        public Accommodation Accommodation { get; set; }

        public PathToPhoto PathToPhoto { get; set; } 

        public FileExtension FileExtension { get; set; }

        public Caption Caption { get; set; }

        public virtual ICollection<PhotoOfAccommodationToTypeOfRoom> ToTypesOfRooms { get; set; }
    }
}
