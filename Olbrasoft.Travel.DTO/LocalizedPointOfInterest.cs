namespace Olbrasoft.Travel.DTO
{
    public class LocalizedPointOfInterest : BaseLocalizedRegion
    {

        public User Creator { get; set; }

        public virtual PointOfInterest PointOfInterest { get; set; }

        public virtual Language Language { get; set; }
    }
}
