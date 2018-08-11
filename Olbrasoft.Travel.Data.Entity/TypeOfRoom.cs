using System.Collections.Generic;

namespace Olbrasoft.Travel.Data.Entity
{
    public class TypeOfRoom : CreatorInfo, IHaveEanId<int>
    {
        //// ReSharper disable once InconsistentNaming
        //public int EANHotelID { get; set; }

        //// ReSharper disable once InconsistentNaming
        //public int RoomTypeID { get; set; }

        //[StringLength(5)]
        //public string LanguageCode { get; set; }

        //[StringLength(256)]
        //public string RoomTypeImage { get; set; }

        //[StringLength(200)]
        //public string RoomTypeName { get; set; }

        //public string RoomTypeDescription { get; set; }

        public int AccommodationId { get; set; }
        public int EanId { get; set; } = int.MinValue;
        public virtual Accommodation Accommodation { get; set; }
       
        public virtual ICollection<LocalizedTypeOfRoom> LocalizedTypesOfRooms { get; set; }

        public virtual ICollection<PhotoOfAccommodationToTypeOfRoom> PhotosOfAccommodationsToTypesOfRooms { get; set; }
    }
}
