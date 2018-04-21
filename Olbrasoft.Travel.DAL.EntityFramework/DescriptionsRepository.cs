using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class DescriptionsRepository : SharpRepository.EfRepository.EfRepository<Description, int, int, int>, IDescriptionsRepository
    {
        public event EventHandler Saved;

        private HashSet<Tuple<int, int, int>> _keys;

        protected HashSet<Tuple<int, int, int>> Keys
        {
            get
            {
                return _keys ?? (_keys = new HashSet<Tuple<int, int, int>>(AsQueryable()
                           .Select(d => new { d.AccommodationId, d.TypeOfDescriptionId, d.LanguageId }).ToArray()
                           .Select(k =>
                               new Tuple<int, int, int>(k.AccommodationId, k.TypeOfDescriptionId, k.LanguageId))));
            }

            private set => _keys = value;
        }


        public DescriptionsRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public void BulkSave(IEnumerable<Description> descriptions, int batchSize, params Expression<Func<Description, object>>[] ignorePropertiesWhenUpdating)
        {
            var forInsert = new Queue<Description>();
            var forUpdate = new Queue<Description>();

            foreach (var description in descriptions)
            {
                if (Keys.Contains(new Tuple<int, int, int>(description.AccommodationId, description.TypeOfDescriptionId,
                    description.LanguageId)))
                {
                    forUpdate.Enqueue(description);
                }
                else
                {
                    forInsert.Enqueue(description);
                }
            }

            if (forInsert.Count > 0)
            {
                Context.BulkInsert(forInsert, OnSaved, batchSize);
            }

            if (forUpdate.Count > 0)
            {
                Context.BulkUpdate(forUpdate, OnSaved, batchSize, ignorePropertiesWhenUpdating);
            }
        }

        public void BulkSave(IEnumerable<Description> descriptions, params Expression<Func<Description, object>>[] ignorePropertiesWhenUpdating)
        {
            BulkSave(descriptions, 90000, ignorePropertiesWhenUpdating);
        }



        protected void OnSaved(EventArgs eventArgs)
        {
            var handler = Saved;
            handler?.Invoke(this, eventArgs);
            ClearCache();
        }

        public void ClearCache()
        {
            Keys = null;
        }


    }
}
