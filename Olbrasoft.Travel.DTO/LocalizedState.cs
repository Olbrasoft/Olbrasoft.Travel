namespace Olbrasoft.Travel.DTO
{
    /// <summary>
    /// Localized information on States and Provinces
    /// </summary>
    public class LocalizedState : LocalizedRegionWithNameAndLongName
    {
        public virtual State State { get; set; }
    }
    
}