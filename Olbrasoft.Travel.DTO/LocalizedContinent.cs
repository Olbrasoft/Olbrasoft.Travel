namespace Olbrasoft.Travel.DTO
{
    public class LocalizedContinent : LocalizedRegionWithNameAndLongName
    {
        public User Creator { get; set; }

        public virtual Continent Continent { get; set; }

        public virtual Language Language { get; set; }
    }
}