using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface ILocalizedRepository<T> : IBulkRepository<T>, IBaseRepository<T, int, int> 
        where T : Localized
    {
        //bool Exists(int languageId);
        //IEnumerable<int> FindIds(int languageId);     
        void BulkSave(IEnumerable<T> entities, int batchSize ,params Expression<Func<T, object>>[] ignorePropertiesWhenUpdating);


    }
}