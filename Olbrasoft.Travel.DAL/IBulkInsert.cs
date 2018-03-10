using System.Collections.Generic;

namespace Olbrasoft.Travel.DAL
{
    public interface IBulkInsert<in T>:ISaved
    {
        void BulkInsert(IEnumerable<T> entities);
    }
}