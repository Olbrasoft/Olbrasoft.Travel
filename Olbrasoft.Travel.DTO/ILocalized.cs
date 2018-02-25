namespace Olbrasoft.Travel.DTO
{
    public interface ILocalized
    {
        int SupportedCultureId { get; set; }
        SupportedCulture SupportedCulture { get; set; }
    }
}