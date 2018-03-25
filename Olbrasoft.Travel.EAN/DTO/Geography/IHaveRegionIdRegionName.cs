namespace Olbrasoft.Travel.EAN.DTO.Geography
{
    public interface IHaveRegionIdRegionName : IHaveRegionId
    {
        string RegionName { get; set; }
    }
}