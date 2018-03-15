namespace Olbrasoft.Travel.DTO
{
    public class BaseGeo  : Creator, IMapToPartners<long> 
    {
        public long EanId { get; set; } = long.MinValue;
    
    }

}