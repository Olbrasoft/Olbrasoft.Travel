using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;

namespace Olbrasoft.Travel.DTO
{
    public  class Region:TravelEntity
    {
       
        [Key]
        public int Id { get; set; }
        public int TypeOfRegionId { get; set; }
        public int? SubClassOfRegionId { get; set;}
        public long? EanRegionId { get; set; }
        public DbGeography Coordinates { get; set; }

        public TypeOfRegion TypeOfRegion { get; set; }
        public SubClassOfRegion SubClassOfRegion { get; set; }
        public ICollection<LocalizedRegion> LocalizedRegions { get; set; }


        public ICollection<RegionToRegion> ToParentRegions { get; set; }
        public ICollection<RegionToRegion> ToChildRegions { get; set; }

        //public ICollection<Region> ParentRegions { get; set; }
        //public ICollection<Region> ChildRegions { get; set; }
    }
}
