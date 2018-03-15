namespace Olbrasoft.Travel.DTO
{
    public class Geo  : CreatorInfo, IMapToPartners<long> 
    {
        public long EanId { get; set; } = long.MinValue;
    
    }

}