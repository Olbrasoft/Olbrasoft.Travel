namespace Olbrasoft.Travel.DTO
{
    public class LocalizedCountry: BaseLocalizedRegion
    {
        public User Creator { get; set; }

        public virtual Country Country { get; set; }

        public virtual Language Language { get; set; }
    }
}