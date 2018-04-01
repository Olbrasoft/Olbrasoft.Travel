using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Travel.EAN.DTO.Property
{
    /// <summary>
    /// https://support.ean.com/hc/en-us/articles/115005782669
    /// File Name: HotelImageList.txt
    /// Zip File Name: https://www.ian.com/affiliatecenter/include/V2/HotelImageList.zip 
    /// This file contains image URLs for all properties in the EAN system.
    /// Images have default size attributes of 350x350.
    /// The DefaultImage bit is set to signify the primary image for the property, also known as the hero image.
    /// This image is typically used as the thumbnail in search results or as the first image to appear in a photo gallery.
    /// Images are returned in the recommended display order with the hero image first, then grouped by the subject of the image(e.g.entrance, lobby, guestroom, etc).
    /// </summary>
    public class HotelImage : PathToHotelImage
    {
       
        [StringLength(70)]
        public string Caption { get; set; }
        
        public int Width { get; set; }

        public int Height { get; set; }

        public int ByteSize { get; set; }
        
        // ReSharper disable once InconsistentNaming
        public string ThumbnailURL { get; set; }

        public bool DefaultImage { get; set; }
    }
}