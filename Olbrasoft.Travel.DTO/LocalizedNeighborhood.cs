namespace Olbrasoft.Travel.DTO
{
    public class LocalizedNeighborhood : BaseLocalizedRegion
    {
        public virtual Neighborhood Neighborhood { get; set; }

        public User Creator { get; set; }

        public virtual Language Language { get; set; }
    }
}