using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface ILocalizedRepository<T> : IBaseRepository<T, int, int>,IBulk<T>
        where T : BaseLocalized
    {
        bool Exists(int languageId);
        IEnumerable<int> FindIds(int languageId);     
    }
}