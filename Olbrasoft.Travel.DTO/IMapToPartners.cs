namespace Olbrasoft.Travel.DTO
{
    
    public interface IMapToPartners<TEanId>
    {
        int Id { get; set; }
        TEanId EanId { get; set; }
    }

}