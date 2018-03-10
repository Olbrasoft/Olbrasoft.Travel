using System.Collections.Generic;
using System.Linq;
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


        public new void Add(T entity)
        {
            if (entity.EanRegionId == long.MinValue)
            {
                entity.EanRegionId = MinEanRegionId;
            }

            base.Add(entity);
        }

        private IEnumerable<T> Rebuild(IEnumerable<T> entities)
        {
            var entitiesArray = entities as T[] ?? entities.ToArray();

            if (entitiesArray.All(p => p.EanRegionId != long.MinValue)) return entitiesArray;

            foreach (var pointOfInterest in entitiesArray.Where(p => p.EanRegionId == long.MinValue))
            {
                pointOfInterest.EanRegionId = MinEanRegionId;
                _minEanRegionId = (_minEanRegionId - 1);
            }

            return entitiesArray;
        }

        public new void Add(IEnumerable<T> entities)
        {
            base.Add(Rebuild(entities));
        }

        public new void BulkInsert(IEnumerable<T> entities)
        {
            base.BulkInsert(Rebuild(entities));
        }

        public new void BulkUpdate(IEnumerable<T> entities)
        {
            base.BulkUpdate(Rebuild(entities));
        }
    }
}
