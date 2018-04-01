using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class MappedEntitiesRepository<T> : BaseRepository<T>, IMappedEntitiesRepository<T> where T : CreationInfo, IHaveEanId<int>
    {
        private int _minEanId = int.MinValue;
        private HashSet<int> _eanIds;
        private IReadOnlyDictionary<int, int> _eanIdsToIds;


        public HashSet<int> EanIds
        {
            get
            {
                return _eanIds ?? (_eanIds = _eanIdsToIds != null
                           ? new HashSet<int>(_eanIdsToIds.Keys)
                           : new HashSet<int>(AsQueryable().Where(p => p.EanId >= 0).Select(p => p.EanId)));
            }

            private set => _eanIds = value;
        }

        public IReadOnlyDictionary<int, int> EanIdsToIds
        {
            get => _eanIdsToIds ?? (_eanIdsToIds = AsQueryable().Where(toa => toa.EanId >= 0)
                       .Select(toa => new { toa.EanId, toa.Id }).ToDictionary(k => k.EanId, v => v.Id));

            private set => _eanIdsToIds = value;
        }

        private int MinEanId
        {
            get
            {
                if (_minEanId != int.MinValue) return _minEanId;
                if (Exists(region => region.EanId < 0))
                {
                    _minEanId = Min(region => region.EanId) - 1;
                }
                else
                {
                    _minEanId = -1;
                }
                return _minEanId;
            }

            set => _minEanId = value;
        }

        public MappedEntitiesRepository(DbContext context) : base(context)
        {
        }

        public void BulkSave(IEnumerable<T> entities, params Expression<Func<T, object>>[] ignorePropertiesWhenUpdating)
        {
            entities = Rebuild(entities.ToArray());

            if (entities.Any(region => region.Id == 0))
            {
                BulkInsert(entities.Where(region => region.Id == 0));
            }

            if (entities.Any(region => region.Id != 0))
            {
                BulkUpdate(entities.Where(region => region.Id != 0), ignorePropertiesWhenUpdating);
            }
        }
        
        protected T[] Rebuild(T[] entities)
        {
            entities = AddingIdsOnDependingEanIds(entities);
            entities = OverrideEanIds(entities);

            return entities;
        }

        protected T[] AddingIdsOnDependingEanIds(T[] entities)
        {
            foreach (var typeOfAccommodation in entities.Where(p => p.EanId >= 0 && p.Id == 0))
            {
                if (!EanIdsToIds.TryGetValue(typeOfAccommodation.EanId, out var id)) continue;
                typeOfAccommodation.Id = id;
            }
            return entities;
        }

        protected T[] OverrideEanIds(T[] entities)
        {
            if (entities.All(r => r.EanId != int.MinValue)) return entities;

            foreach (var region in entities.Where(p => p.EanId == int.MinValue))
            {
                region.EanId = MinEanId;
                MinEanId = MinEanId - 1;
            }

            return entities;
        }

        public override void ClearCache()
        {
            MinEanId = int.MinValue;
            EanIds = null;
            EanIdsToIds = null;
            base.ClearCache();
        }
    }
}