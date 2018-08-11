namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.DataTransferObject.Geography
{
    public interface IHaveRegionIdLatitudeLongitude : IHaveRegionId
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
    }
}