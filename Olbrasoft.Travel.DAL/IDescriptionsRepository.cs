using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IDescriptionsRepository : IBulkRepository<Description>, SharpRepository.Repository.ICompoundKeyRepository<Description, int, int, int>
    {
        void BulkSave(IEnumerable<Description> entities, int batchSize, params Expression<Func<Description, object>>[] ignorePropertiesWhenUpdating);
    }
}