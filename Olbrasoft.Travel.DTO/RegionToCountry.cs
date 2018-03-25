namespace Olbrasoft.Travel.DTO
{
    public class RegionToCountry : CreatorInfo
    {
        public int CountryId { get; set; }
        
        public Region Region { get; set; }

        public Country Country { get; set; }
    }
}