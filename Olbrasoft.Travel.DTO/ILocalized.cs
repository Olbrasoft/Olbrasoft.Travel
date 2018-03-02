namespace Olbrasoft.Travel.DTO
{
    public interface ILocalized
    {
        int LanguageId { get; set; }
        Language Language { get; set; }
    }
}