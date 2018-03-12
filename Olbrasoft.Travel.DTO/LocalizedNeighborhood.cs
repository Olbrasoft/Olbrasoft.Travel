namespace Olbrasoft.Travel.DTO
{
    public class LocalizedNeighborhood : LocalizedRegionWithNameAndLongName
    {
        public virtual Neighborhood Neighborhood { get; set; }

        public User Creator { get; set; }

        public virtual Language Language { get; set; }
    }
}