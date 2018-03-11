using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class BaseRegionsRepository<T> : BaseRepository<T>, IBaseRegionsRepository<T> where T : BaseRegion
    {
        private IEnumerable<long> _eanRegionIds;
        private IReadOnlyDictionary<long, int> _eanRegionIdsToIds;

        public IEnumerable<long> EanRegionIds
        {
            get
            {
                return _eanRegionIds ?? (_eanRegionIds = GetAll(p => p.EanRegionId));
            }
        }

        public IReadOnlyDictionary<long, int> EanRegionIdsToIds
        {
            get
            {
                return _eanRegionIdsToIds ?? (_eanRegionIdsToIds =
                           FindAll(p => p.EanRegionId >= 0, s => new { s.EanRegionId, s.Id })
                               .ToDictionary(k => k.EanRegionId, v => v.Id));
            }
        }

        private long _minEanRegionId = long.MinValue;

        private long MinEanRegionId
        {
            get
            {
                if (_minEanRegionId != long.MinValue) return _minEanRegionId;
                if (Exists(region => region.EanRegionId < 0))
                {
                    _minEanRegionId = Min(poi => poi.EanRegionId) - 1;
                }
                else
                {
                    _minEanRegionId = -1;
                }
                return _minEanRegionId;
            }

        }

        public BaseRegionsRepository(TravelContext context) : base(context)
        {

        }


        public override void ClearCache()
        {
            _eanRegionIds = null;
            _eanRegionIdsToIds = null;
            _minEanRegionId = long.MinValue;

        }


        public new void Add(T baseRegion)
        {
            if (baseRegion.EanRegionId == long.MinValue)
            {
                baseRegion.EanRegionId = MinEanRegionId;
            }

            base.Add(baseRegion);
        }

        private IEnumerable<T> RebuildEanRegionIds(IEnumerable<T> baseRegions)
        {
            var regions = baseRegions as T[] ?? baseRegions.ToArray();

            if (regions.All(p => p.EanRegionId != long.MinValue)) return regions;

            //Parallel.ForEach(regions.Where(p => p.EanRegionId == long.MinValue), baseRegion =>
            //{
            //    baseRegion.EanRegionId = MinEanRegionId;
            //    _minEanRegionId = (_minEanRegionId - 1);
            //});

            foreach (var baseRegion in regions.Where(p => p.EanRegionId == long.MinValue))
            {
                baseRegion.EanRegionId = MinEanRegionId;
                _minEanRegionId = (_minEanRegionId - 1);
            }

            return regions;
        }

        public new void Add(IEnumerable<T> baseRegions)
        {
            base.Add(RebuildEanRegionIds(baseRegions));
        }

        public override void BulkSave(IEnumerable<T> baseRegions)
        {
            var regions = baseRegions as T[] ?? baseRegions.ToArray();

            //Parallel.ForEach(regions.Where(p => p.EanRegionId >= 0 && p.Id == 0),
            //    region =>
            //    {
            //        if (EanRegionIdsToIds.TryGetValue(region.EanRegionId, out var id))
            //        {
            //            region.Id = id;
            //        }
            //    }
            //);


            foreach (var region in regions.Where(p => p.EanRegionId >= 0 && p.Id == 0))
            {
                if (!EanRegionIdsToIds.TryGetValue(region.EanRegionId, out var id)) continue;
                region.Id = id;
            }

            base.BulkSave(RebuildEanRegionIds(regions));
        }
    }
}
