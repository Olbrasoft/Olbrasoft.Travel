namespace Olbrasoft.Travel.DTO
{
    public class LocalizedState : LocalizedRegionWithNameAndLongName
    {
        public virtual State State { get; set; }
    }
    
}