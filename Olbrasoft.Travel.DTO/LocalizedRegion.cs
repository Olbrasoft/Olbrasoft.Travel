namespace Olbrasoft.Travel.DTO
{
    public class LocalizedRegion : BaseLocalizedRegion
    {
        public User Creator { get; set; }

        public virtual Region Region { get; set; }

        public virtual Language Language { get; set; }
    }
}
