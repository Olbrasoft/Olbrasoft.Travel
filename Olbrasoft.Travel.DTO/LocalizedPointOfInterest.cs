namespace Olbrasoft.Travel.DTO
{
    public class LocalizedPointOfInterest : BaseLocalizedRegionWithLongName
    {
        
        public User Creator { get; set; }

        public virtual PointOfInterest PointOfInterest { get; set; }

        public virtual Language Language { get; set; }
    }
}
