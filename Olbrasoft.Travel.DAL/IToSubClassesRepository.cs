using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IToSubClassesRepository<T> : IBaseRepository<T> where T : ToSubClass
    {
        void BulkSave(IEnumerable<T> entitiesToSubClases);
    }
}
