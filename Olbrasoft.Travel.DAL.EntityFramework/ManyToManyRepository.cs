using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class ManyToManyRepository<T> : BaseRepository<T, int, int>, IManyToManyRepository<T> where T : ManyToMany
    {
        private HashSet<Tuple<int, int>> _idsToToIds;

        public HashSet<Tuple<int, int>> IdsToToIds
        {
            get
            {
                if (_idsToToIds != null) return _idsToToIds;
          
                _idsToToIds = new HashSet<Tuple<int, int>>(AsQueryable().Select(p => new {p.Id, p.ToId}).ToArray()
                    .Select(p => new Tuple<int, int>(p.Id, p.ToId)));

                return _idsToToIds;
                
            }

            private set => _idsToToIds = value;
        }


        public ManyToManyRepository(DbContext context) : base(context)
        {
        }

        public override void ClearCache()
        {
            IdsToToIds = null;
        }

        public override void BulkSave(IEnumerable<T> manyToManyEntities, params Expression<Func<T, object>>[] ignorePropertiesWhenUpdating)
        {
            var forInsert = new Dictionary<Tuple<int, int>, T>();

            foreach (var manyToMany in manyToManyEntities)
            {
                var tup = new Tuple<int, int>(manyToMany.Id, manyToMany.ToId);

                if (IdsToToIds.Contains(tup)) continue;

                if (!forInsert.ContainsKey(tup))
                {
                    forInsert.Add(tup, manyToMany);
                }
            }

            BulkInsert(forInsert.Values);
        }

    }
}