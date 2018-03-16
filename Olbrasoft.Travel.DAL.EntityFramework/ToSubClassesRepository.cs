using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class ToSubClassesRepository<T> : BaseRepository<T>, IToSubClassesRepository<T> where T : ToSubClass
    {
        private HashSet<int> _ids;

        protected HashSet<int> Ids
        {
            get
            {
                return _ids ?? (_ids = new HashSet<int>(GetAll(p => p.Id)));
            }
        }

        public ToSubClassesRepository(DbContext context) : base(context)
        {
        }

        public override void ClearCache()
        {
            _ids = null;
        }

        public void BulkSave(IEnumerable<T> entitiesToSubClases  )
        {
            var forInsert = entitiesToSubClases.Where(entityToSubClass => !Ids.Contains(entityToSubClass.Id)); 
            Context.BulkInsert(forInsert, OnSaved);
        }
    }
}