using System.Collections.Generic;
using Olbrasoft.Travel.Data.Entity;

namespace Olbrasoft.Travel.DataAccessLayer
{
    public interface ILanguagesRepository : IBaseRepository<Language>
    {
        IReadOnlyDictionary<string,int> EanLanguageCodesToIds { get; }
    }
}