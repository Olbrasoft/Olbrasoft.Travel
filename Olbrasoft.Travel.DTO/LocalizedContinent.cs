namespace Olbrasoft.Travel.DTO
{
    public class LocalizedContinent : LocalizedRegionWithNameAndLongName
    {
        public virtual Continent Continent { get; set; }

    }
}