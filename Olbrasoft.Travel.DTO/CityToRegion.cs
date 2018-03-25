namespace Olbrasoft.Travel.DTO
{
    public class CityToRegion : CityTo
    {
        public int RegionId { get; set; }

        public Region Region { get; set; }
    }
}