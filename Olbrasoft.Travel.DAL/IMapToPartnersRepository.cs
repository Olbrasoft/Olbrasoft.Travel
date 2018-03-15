using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{

    public interface IMapToPartnersRepository<T,TEanId> : SharpRepository.Repository.IRepository<T> where T: CreatorInfo, IMapToPartners<TEanId>
    {
        IReadOnlyDictionary<TEanId,int> EanIdsToIds { get; }
        void BulkSave(IEnumerable<T> entities);
    }
}
