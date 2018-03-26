using System.Collections.Generic;
using System.Data.Entity.Spatial;

namespace Olbrasoft.Travel.DTO
{
    public class Region : CreatorInfo
    {

        public Region()
        {
            RegionsToTypes = new HashSet<RegionToType>();
            LocalizedRegions = new HashSet<LocalizedRegion>();
        }

        // public int TypeOfRegionId { get; set; }

        public DbGeography Coordinates { get; set; }

        public DbGeography CenterCoordinates { get; set; }

        public long EanId { get; set; } = long.MinValue;

        public ICollection<RegionToType> RegionsToTypes { get; set; }

        public ICollection<LocalizedRegion> LocalizedRegions { get; set; }

        //public TypeOfRegion TypeOfRegion { get; set; }

        //public virtual RegionToSubClass ToSubClass { get; set; }

        public ICollection<RegionToRegion> ToParentRegions { get; set; }

        public ICollection<RegionToRegion> ToChildRegions { get; set; }

        public Country AdditionalCountryProperties { get; set; }

        public Airport AdditionalAirportProperties { get; set; }

        //public ICollection<CityToRegion> CitiesToRegions { get; set; }
        // public virtual ICollection<PointOfInterestToRegion> PointsOfInterestToRegions { get; set; }
        // public virtual ICollection<PointOfInterest> PointsOfInterest { get; set; }
        //public ICollection<Region> ParentRegions { get; set; }
        //public ICollection<Region> ChildRegions { get; set; }
    }
}
