using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class GeoRepository<T> : MapToPartnersRepository<T,long>,IGeoRepository<T> where T : CreatorInfo, IMapToPartners<long> 
    {
        private long _minEanId = long.MinValue;

        private IReadOnlyDictionary<long, int> _eanIdsToIds;

        public override IReadOnlyDictionary<long, int> EanIdsToIds
        {
            get
            {
                return _eanIdsToIds ?? (_eanIdsToIds =
                           FindAll(p => p.EanId >= 0, s => new { EanAirportId = s.EanId, s.Id })
                               .ToDictionary(k => k.EanAirportId, v => v.Id));
            }
        }
        
        private long MinEanId
        {
            get
            {
                if (_minEanId != long.MinValue) return _minEanId;
                if (Exists(region => region.EanId < 0))
                {
                    _minEanId = Min(poi => poi.EanId) - 1;
                }
                else
                {
                    _minEanId = -1;
                }
                return _minEanId;
            }
        }

        public GeoRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override T RebuildPartnersIds(T entity)
        {
            if (entity.EanId == long.MinValue)
            {
                entity.EanId = MinEanId;
            }

            return entity;
        }


        protected override IEnumerable<T> RebuildPartnersIds(IEnumerable<T> entities)
        {
            var entitiesArray = entities as T[] ?? entities.ToArray();

            foreach (var entity in entitiesArray.Where(p => p.EanId >= 0 && p.Id == 0))
            {
                if (!EanIdsToIds.TryGetValue(entity.EanId, out var id)) continue;
                entity.Id = id;
            }

            if (entitiesArray.All(p => p.EanId != long.MinValue)) return entitiesArray;

            foreach (var entity in entitiesArray.Where(p => p.EanId == long.MinValue))
            {
                entity.EanId = MinEanId;
                _minEanId = (_minEanId - 1);
            }
            return entitiesArray;
        }
        
        public override void ClearCache()
        {
            _minEanId = long.MinValue;
            _eanIdsToIds = null;
        }
    }
}