using System.Data.Common;
using System.Data.Entity;

namespace Olbrasoft.EntityFramework.Bulk
{
    public static class ExDbContextTransaction
    {
        public static DbTransaction GetDbTransaction(this DbContextTransaction transaction)
        {
            return transaction.UnderlyingTransaction;

        }
    }
}
