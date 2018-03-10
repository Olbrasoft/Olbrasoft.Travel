﻿using System.Collections.Generic;
using System.Linq;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class ManyToManyRepository<T> : BaseRepository<T, int, int>, IManyToManyRepository<T> where T : ManyToMany
    {
        private IReadOnlyDictionary<int, int> _idsToToIds;
        
        public IReadOnlyDictionary<int, int> IdsToToIds
        {
            get
            {
                return _idsToToIds ??
                       (_idsToToIds = GetAll(p => new { p.Id, p.ToId }).ToDictionary(k => k.Id, v => v.ToId));
            }
        }

        public ManyToManyRepository(TravelContext context) : base(context)
        {
            
        }

        public override void ClearCache()
        {
            _idsToToIds = null;
        }


    }
}