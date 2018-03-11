using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                return _idsToToIds ??
                       (
                           _idsToToIds = new HashSet<Tuple<int, int>>( GetAll(p => new Tuple<int,int>( p.Id, p.ToId)))
                        ) ;
            }
        }

        public ManyToManyRepository(DbContext context) : base(context)
        {
        }

        public override void ClearCache()
        {
            _idsToToIds = null;
        }

        public override void BulkSave(IEnumerable<T> manyToManyEntities)
        {
            var forInsert = new Dictionary<Tuple<int, int>, T>();
            
            foreach (var manyToMany in manyToManyEntities)
            {
                var tup = new Tuple<int, int>(manyToMany.Id, manyToMany.ToId);

                if (IdsToToIds.Contains(tup)) continue;
                
                if (forInsert.ContainsKey(tup)) continue;

                forInsert.Add(tup,manyToMany);
            }

            BulkInsert(forInsert.Values);
        }

    }
}