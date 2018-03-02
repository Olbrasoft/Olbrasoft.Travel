using System.Data.Common;
using System.Data.Entity;

namespace Olbrasoft.EntityFramework.Bulk
{
    public static class ExDatabase
    {
        public static DbConnection GetDbConnection(this Database database)
        {
            return database.Connection;
        }

    }
}
