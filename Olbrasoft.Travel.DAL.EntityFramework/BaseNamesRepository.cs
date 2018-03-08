using System;
using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class BaseNamesRepository<T>: KeyIdRepository<T>, IBaseNamesRepository<T> where T : BaseName
    {
        private IReadOnlyDictionary<string, int> _namesToIds;

        public IReadOnlyDictionary<string, int> NamesToIds
        {
            get
            {
                return _namesToIds ??
                       (_namesToIds = GetAll(p => new {p.Name, p.Id}).ToDictionary(k => k.Name, v => v.Id));
            }
        }

        public BaseNamesRepository(TravelContext travelContext) : base(travelContext)
        {
            OnSaved += ClearCache;
        }

        private void ClearCache(object sender, EventArgs eventArgs)
        {
            ClearCache();
        }

        public new void ClearCache()
        {
            _namesToIds = null;
            base.ClearCache();
        }


    }
}