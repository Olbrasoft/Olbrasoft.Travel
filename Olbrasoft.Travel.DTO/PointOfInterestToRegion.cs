namespace Olbrasoft.Travel.DTO
{
    public class PointOfInterestToRegion : ManyToMany
    {
        public virtual Region Region { get; set; }

        public virtual PointOfInterest PointOfInterest { get; set; }

        public virtual User Creator { get; set; }
    }
}