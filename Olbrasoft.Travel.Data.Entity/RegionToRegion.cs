namespace Olbrasoft.Travel.Data.Entity
{
    public class RegionToRegion : ManyToMany
    {
        public virtual Region Region { get; set; }

        public virtual Region ParentRegion { get; set; }
       
    }

   

}
