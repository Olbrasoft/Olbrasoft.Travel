namespace Olbrasoft.Travel.DTO
{
    public class CityToSubClass : CityTo
    {
        public int SubClassId { get; set; }
        public SubClass SubClass { get; set; }
    }
}