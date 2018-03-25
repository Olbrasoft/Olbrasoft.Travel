namespace Olbrasoft.Travel.EAN.DTO.Geography
{
    public interface IHaveRegionIdLatitudeLongitude : IHaveRegionId
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
    }
}