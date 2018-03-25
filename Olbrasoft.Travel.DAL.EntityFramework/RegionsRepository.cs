using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class RegionsRepository : BaseRepository<Region>, IRegionsRepository
    {
        private long _minEanRegionId = long.MinValue;
       
        private IReadOnlyDictionary<long, int> _eanIdsToIds;


        public IReadOnlyDictionary<long, int> EanIdsToIds
        {
            get
            {
                return _eanIdsToIds ?? (_eanIdsToIds = AsQueryable().Where(p => p.EanId >= 0)
                           .Select(r => new {r.EanId, r.Id})
                           .ToDictionary(k => k.EanId, v => v.Id));

                //return _eanIdsToIds ?? (_eanIdsToIds =
                //           FindAll(p => p.EanId >= 0, s => new { EanRegionId = s.EanId, s.Id })
                //               .ToDictionary(k => k.EanRegionId, v => v.Id));
            }

            private set => _eanIdsToIds = value;
        }
        
        private long MinEanRegionId
        {
            get
            {
                if (_minEanRegionId != long.MinValue) return _minEanRegionId;
                if (Exists(region => region.EanId < 0))
                {
                    _minEanRegionId = Min(region => region.EanId) - 1;
                }
                else
                {
                    _minEanRegionId = -1;
                }
                return _minEanRegionId;
            }

            set => _minEanRegionId = value;
        }

       

        public RegionsRepository(DbContext dbContext) : base(dbContext)
        {
        }


        public new void Add(Region region)
        {
            region = Rebuild(new[] {region}).FirstOrDefault();
            base.Add(region);
        }
        

        public void BulkSave(IEnumerable<Region> regions, params Expression<Func<Region, object>>[] ignorePropertiesWhenUpdating)
        {
            regions = Rebuild(regions.ToArray());

            if (regions.Any(region => region.Id == 0))
            {
                BulkInsert(regions.Where(region => region.Id == 0));
            }

            if (regions.Any(region => region.Id != 0))
            {
                BulkUpdate(regions.Where(region => region.Id != 0), ignorePropertiesWhenUpdating);
            }
        }
        
        protected Region[] Rebuild(Region[] regions)
        {
            regions = AddingIdsOnDependingRegionIds(regions);
            regions = OverrideRegionIds(regions);
           
            return regions;
        }
        
        protected Region[] AddingIdsOnDependingRegionIds(Region[] regions)
        {
            foreach (var region in regions.Where(p => p.EanId >= 0 && p.Id == 0))
            {
                if (!EanIdsToIds.TryGetValue(region.EanId, out var id)) continue;
                region.Id = id;
            }
            return regions;
        }
        
        protected Region[] OverrideRegionIds(Region[] regions)
        {
            if (regions.All(r => r.EanId != long.MinValue)) return regions;

            foreach (var region in regions.Where(p => p.EanId == long.MinValue))
            {
                region.EanId = MinEanRegionId;
                MinEanRegionId = MinEanRegionId - 1;
            }

            return regions;
        }
        
        public override void ClearCache()
        {
            MinEanRegionId = long.MinValue;
            EanIdsToIds = null;
           base.ClearCache();
        }
    }
}