using System;
using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class BaseNamesRepository<T>: BaseRepository<T>, IBaseNamesRepository<T> where T : BaseName
    {
        private IReadOnlyDictionary<string, int> _namesToIds;
        private IEnumerable<string> _names;

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
                       (_namesToIds = GetAll(p => new {p.Name, p.Id}).ToDictionary(k => k.Name, v => v.Id));
            }
        }

        public BaseNamesRepository(TravelContext context) : base(context)
        {
            
        }
        

        public override void ClearCache()
        {
            _names = null;
            _namesToIds = null;
        }


    }
}