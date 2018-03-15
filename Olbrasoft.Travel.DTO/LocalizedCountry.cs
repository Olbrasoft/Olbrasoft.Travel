namespace Olbrasoft.Travel.DTO
{
    public class LocalizedCountry : BaseLocalizedRegion
    {
        public virtual Country Country { get; set; }
    }
}