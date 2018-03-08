namespace Olbrasoft.Travel.DTO
{
    public class BasePointOfInterest : BaseRegion
    {
        public bool Shadow { get; set; }

        public int? SubClassId { get; set; }
    }
}