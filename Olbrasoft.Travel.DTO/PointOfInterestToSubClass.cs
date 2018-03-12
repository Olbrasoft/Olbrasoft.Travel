namespace Olbrasoft.Travel.DTO
{
    public class PointOfInterestToSubClass : ToSubClass
    {
       
        public virtual PointOfInterest PointOfInterest { get; set; }

        public virtual SubClass SubClass { get; set; }

        public virtual User Creator { get; set; }
    }
}