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
                if (_names != null) return _names;
                _names = _namesToIds?.Keys ?? GetAll(p => p.Name);
                return _names;
            }

            private set => _names = value;
        }

        public IReadOnlyDictionary<string, int> NamesToIds
        {
            get
            {
                return _namesToIds ??
                       (_namesToIds = GetAll(p => new { p.Name, p.Id }).ToDictionary(k => k.Name, v => v.Id));
            }

            private set => _namesToIds = value;
        }


        public TypesRepository(DbContext context) : base(context)
        {
        }

        public void BulkSave(IEnumerable<T> entities, int batchSize, params Expression<Func<T, object>>[] ignorePropertiesWhenUpdating)
        {
            Context.BulkInsert(ForInsert(entities), OnSaved, batchSize);
        }

        public void BulkSave(IEnumerable<T> entities, params Expression<Func<T, object>>[] ignorePropertiesWhenUpdating)
        {
            BulkSave(entities, 90000, ignorePropertiesWhenUpdating);
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
            Names = null;
            NamesToIds = null;
        }


    }
}