using System.Collections.Generic;

namespace Olbrasoft.Travel.DAL
{
    public interface IBulk<in T> :IBulkInsert<T>
    {
        void BulkUpdate(IEnumerable<T> entities);
    }
}