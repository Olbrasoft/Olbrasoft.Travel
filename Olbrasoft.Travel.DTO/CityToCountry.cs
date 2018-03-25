namespace Olbrasoft.Travel.DTO
{
    public class CityToCountry : CityTo
    {
        public int CountryId { get; set; }
        
        public Country Country { get; set; }
    }
}