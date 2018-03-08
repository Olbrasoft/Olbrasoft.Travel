using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IKeyIdRepository<T> : ITravelRepository<T> where T : class, IKeyId
    {
        void BulkSave(IEnumerable<T> entities);
    }
}