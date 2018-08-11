namespace Olbrasoft.Travel.ExpediaAffiliateNetwork.DataTransferObject.Geography
{
    public interface IHaveRegionIdRegionName : IHaveRegionId
    {
        string RegionName { get; set; }
    }
}