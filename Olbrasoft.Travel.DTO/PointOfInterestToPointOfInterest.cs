namespace Olbrasoft.Travel.DTO
{
    public class PointOfInterestToPointOfInterest:ManyToMany
    {

        public virtual PointOfInterest PointOfInterest { get; set; }

        public virtual PointOfInterest ParentPointOfInterest { get; set; }

      
        public virtual User Creator { get; set; }

    }
}