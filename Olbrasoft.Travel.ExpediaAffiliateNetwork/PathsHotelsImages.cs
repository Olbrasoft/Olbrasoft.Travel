using System.ComponentModel.DataAnnotations;
using Olbrasoft.Travel.ExpediaAffiliateNetwork.DataTransferObject.Property;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork
{
    public class PathsHotelsImages : BaseParser<PathToHotelImage>, IPathsHotelsImagesParser
    {
        public override PathToHotelImage Parse(string[] items)
        {
            throw new System.NotImplementedException();
        }

        public override bool TryParse(string line, out PathToHotelImage entita)
        {
            var properties= line.Split('|');

            if (!int.TryParse(properties[0], out var eanHotelId))
            {
                entita = null;
                return false;
            }

            entita= new PathToHotelImage
            {
                EANHotelID =eanHotelId,
                URL = properties[2]
            };
            return Validator.TryValidateObject(entita, new ValidationContext(entita), null, true);
        }
        
    }
}