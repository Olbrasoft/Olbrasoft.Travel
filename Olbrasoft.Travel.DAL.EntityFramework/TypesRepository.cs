using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class TypesRepository<T> : BaseRepository<T>, ITypesRepository<T> where T : CreationInfo, IHaveName
    {
        private IReadOnlyDictionary<string, int> _namesToIds;
        private IEnumerable<string> _names;

        public int GetId(string name)
        {
            if (_namesToIds == null) return Find(tor => tor.Name == name).Id;

            return NamesToIds.TryGetValue(name, out var id) ? id : Find(tor => tor.Name == name).Id;
        }

        public IEnumerable<string> Names
        {
            get
            {
                return _names ?? (_names = GetAll(p => p.Name));
            }
        }

        public IReadOnlyDictionary<string, int> NamesToIds
        {
            get
            {
                return _namesToIds ??
                       (_namesToIds = GetAll(p => new { p.Name, p.Id }).ToDictionary(k => k.Name, v => v.Id));
            }
        }
        

        public TypesRepository(DbContext context) : base(context)
        {
        }

        
        public void BulkSave(IEnumerable<T> entities, params Expression<Func<T, object>>[] ignorePropertiesWhenUpdating)
        {
           Context.BulkInsert(ForInsert(entities), OnSaved);
        }

        private IEnumerable<T> ForInsert(IEnumerable<T> entities)
        {
            var forInsert = new HashSet<T>();
            var stored = new HashSet<string>(Names);
            foreach (var entity in entities)
            {
                if (!stored.Contains(entity.Name) && !forInsert.Contains(entity))
                {
                    forInsert.Add(entity);
                }
            }

            return forInsert;
        }


        public override void ClearCache()
        {
            _names = null;
            _namesToIds = null;
        }


    }
}