using System;
using System.Collections.Generic;
using System.Linq;
using Olbrasoft.EntityFramework.Bulk;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class ManyToManyRepository<T> : SharpRepository.EfRepository.EfRepository<T, int, int>, IManyToManyRepository<T> where T : ManyToMany
    {
        private IReadOnlyDictionary<int, int> _idsToToIds;

        protected new readonly TravelContext Context;
        
        public event EventHandler<EventArgs> OnSaved;

        public IReadOnlyDictionary<int, int> IdsToToIds
        {
            get
            {
                return _idsToToIds ??
                       (_idsToToIds = GetAll(p => new {p.Id, p.ToId}).ToDictionary(k => k.Id, v => v.ToId));
            }
        }
        
        public ManyToManyRepository( TravelContext travelContext) : base(travelContext)
        {

            Context = travelContext;
            OnSaved += ClearCache;
        }

        private void ClearCache(object sender, EventArgs eventArgs)
        {
            ClearCache();
        }

    
        public void ClearCache()
        {
            _idsToToIds = null;
        }

        public new void Add(IEnumerable<T> entities)
        {
            base.Add(entities);
            OnSaved?.Invoke(this, new EventArgs());
        }

        public new void Add(T entity)
        {
            base.Add(entity);
            OnSaved?.Invoke(this, new EventArgs());
        }


        public virtual void BulkInsert(IEnumerable<T> entities)
        {
            var batchesToInsert = TravelRepository<T>.SplitList(entities, 90000);

            foreach (var batch in batchesToInsert)
            {
                Context.BulkInsert(batch,
                    new BulkConfig
                    {
                        BatchSize = 45000,
                        BulkCopyTimeout = 480,
                        IgnoreColumns = new HashSet<string>(new[] { "DateAndTimeOfCreation" })

                    });

                OnSaved?.Invoke(this, new EventArgs());
            }
        }
    }
}