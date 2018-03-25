using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IOneToManyRepository<T> : IBaseRepository<T> where T : CreationInfo
    {
        void BulkSave(IEnumerable<T> entitiesToEntities);
    }
}
