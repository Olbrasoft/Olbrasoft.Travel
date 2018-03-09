namespace Olbrasoft.Travel.DTO
{
    public class RegionToRegion : ManyToMany
    {
     
        public virtual Region Region { get; set; }

        public virtual Region ParentRegion { get; set; }
        
        public virtual User Creator { get; set; }
    }
}
