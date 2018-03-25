namespace Olbrasoft.Travel.DTO
{
    public class RegionToSubClass : CreatorInfo
    {

        public Region Region { get; set; }

        public int SubClassId { get; set; }

        public SubClass SubClass { get; set; }

    }
}