using System;
using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IManyToManyRepository<T> : SharpRepository.Repository.ICompoundKeyRepository<T, int,int> where T : ManyToMany
    {
        event EventHandler<EventArgs> OnSaved;
        IReadOnlyDictionary<int,int> IdsToToIds { get; }
        void BulkInsert(IEnumerable<T> entities);
    }
}