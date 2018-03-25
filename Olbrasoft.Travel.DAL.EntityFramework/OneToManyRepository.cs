using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class OneToManyRepository<T> : BaseRepository<T>, IOneToManyRepository<T> where T : CreationInfo
    {
        private HashSet<int> _ids;

        protected HashSet<int> Ids
        {
            get
            {
                return _ids ?? (_ids = new HashSet<int>(GetAll(p => p.Id)));
            }
        }

        public OneToManyRepository(DbContext context) : base(context)
        {

        }


        public new void ClearCache()
        {
            _ids = null;
            base.ClearCache();
        }

        public void BulkSave(IEnumerable<T> entitiesToEntities)
        {
            var forInsert = entitiesToEntities.Where(entityToSubClass => !Ids.Contains(entityToSubClass.Id));
            Context.BulkInsert(forInsert, OnSaved);
        }
    }
}