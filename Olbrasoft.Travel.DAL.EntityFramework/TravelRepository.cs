using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class TravelRepository<T> : BaseRepository<T>, ITravelRepository<T> where T : CreationInfo
    {
        private IEnumerable<int> _ids;

        protected IEnumerable<int> Ids
        {
            get { return _ids ?? (_ids = GetAll(p => p.Id)); }

            private set => _ids = value;
        }

        public TravelRepository(DbContext context) : base(context)
        {
        }

        public void BulkSave(IEnumerable<T> entities, params Expression<Func<T, object>>[] ignorePropertiesWhenUpdating)
        {
            var forInsert = new Queue<T>();
            var forUpdate = new Queue<T>();
            var ids = new HashSet<int>(Ids);

            foreach (var country in entities)
            {
                if (ids.Contains(country.Id))
                {
                    forUpdate.Enqueue(country);
                }
                else
                {
                    forInsert.Enqueue(country);
                }
            }

            if (forInsert.Count > 0) BulkInsert(forInsert);

            if (forUpdate.Count > 0) BulkUpdate(forUpdate,  ignorePropertiesWhenUpdating);
        }

        public override void ClearCache()
        {
            Ids = null;
            base.ClearCache();
        }

    }
}