using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public abstract class MapToPartnersRepository<T, TEanId> : BaseRepository<T>, IMapToPartnersRepository<T, TEanId> where T : CreatorInfo, IMapToPartners<TEanId>
    {
        public abstract IReadOnlyDictionary<TEanId, int> EanIdsToIds { get; }

        protected MapToPartnersRepository(DbContext context) : base(context)
        {

        }

        protected abstract IEnumerable<T> RebuildPartnersIds(IEnumerable<T> entities);

        protected abstract T RebuildPartnersIds(T entity);
        
        public new void Add(T entity)
        {
            base.Add(RebuildPartnersIds(entity));
        }

        public new void Add(IEnumerable<T> entities)
        {
            base.Add(RebuildPartnersIds(entities));
        }

        public override void BulkSave(IEnumerable<T> entities, params Expression<Func<T,object>>[] ignorePropertiesWhenUpdating)
        {
            base.BulkSave(RebuildPartnersIds(entities),ignorePropertiesWhenUpdating);
        }
        
    }
}