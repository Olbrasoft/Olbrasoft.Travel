using System.Collections.Generic;
using Olbrasoft.Travel.Data.Entity;

namespace Olbrasoft.Travel.DataAccessLayer
{
    public interface ILocalizedCaptionsRepository : ILocalizedRepository<LocalizedCaption>
    {
        IReadOnlyDictionary<string, int> GetLocalizedCaptionsTextsToIds(int languageId);
    }
}