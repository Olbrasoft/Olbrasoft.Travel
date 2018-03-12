﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Olbrasoft.EntityFramework.Bulk
{
    public enum OperationType
    {
        Insert,
        InsertOrUpdate,
        Update,
        Delete,
    }

    internal static class SqlBulkOperation
    {
        public static void Insert<T>(DbContext context, IList<T> entities, TableInfo tableInfo, Action<decimal> progress)
        {
            var sqlConnection = OpenAndGetSqlConnection(context);
            var transaction = context.Database.CurrentTransaction;
            try
            {
                using (var sqlBulkCopy = GetSqlBulkCopy(sqlConnection, transaction, tableInfo.BulkConfig.KeepIdentity))
                {
                    sqlBulkCopy.BulkCopyTimeout = 960;
                    tableInfo.SetSqlBulkCopyConfig(sqlBulkCopy, entities, progress);
                    using (var reader = ObjectReaderEx.Create(entities, tableInfo.ShadowProperties, context, tableInfo.PropertyColumnNamesDict.Keys.ToArray()))
                    {
                        sqlBulkCopy.WriteToServer(reader);
                    }
                }
            }
            finally
            {
                if (transaction == null)
                {
                    sqlConnection.Close();
                }
            }
        }

        public static async Task InsertAsync<T>(DbContext context, IList<T> entities, TableInfo tableInfo, Action<decimal> progress)
        {
            var sqlConnection = await OpenAndGetSqlConnectionAsync(context);
            var transaction = context.Database.CurrentTransaction;
            try
            {
                using (var sqlBulkCopy = GetSqlBulkCopy(sqlConnection, transaction, tableInfo.BulkConfig.KeepIdentity))
                {
                    tableInfo.SetSqlBulkCopyConfig(sqlBulkCopy, entities, progress);
                    using (var reader = ObjectReaderEx.Create(entities, tableInfo.ShadowProperties, context, tableInfo.PropertyColumnNamesDict.Keys.ToArray()))
                    {
                        await sqlBulkCopy.WriteToServerAsync(reader).ConfigureAwait(false);
                    }
                }
            }
            finally
            {
                if (transaction == null)
                {
                    sqlConnection.Close();
                }
            }
        }
        
        public static void Merge<T>(DbContext context, IList<T> entities, TableInfo tableInfo, OperationType operationType, Action<decimal> progress) where T : class
        {
            tableInfo.InsertToTempTable = true;
            tableInfo.CheckHasIdentity(context);

            context.Database.CommandTimeout = 960;

            context.Database.ExecuteSqlCommand(SqlQueryBuilder.CreateTableCopy(tableInfo.FullTableName, tableInfo.FullTempTableName));
            
            if (tableInfo.BulkConfig.SetOutputIdentity)
            {
                context.Database.ExecuteSqlCommand(SqlQueryBuilder.CreateTableCopy(tableInfo.FullTableName, tableInfo.FullTempOutputTableName));
            }
            try
            {
                Insert(context, entities, tableInfo, progress);
                context.Database.ExecuteSqlCommand(SqlQueryBuilder.MergeTable(tableInfo, operationType));
                context.Database.ExecuteSqlCommand(SqlQueryBuilder.DropTable(tableInfo.FullTempTableName));

                if (!tableInfo.BulkConfig.SetOutputIdentity || !tableInfo.HasSinglePrimaryKey) return;
                try
                {
                    tableInfo.UpdateOutputIdentity(context, entities);
                    var dp = SqlQueryBuilder.DropTable(tableInfo.FullTempOutputTableName);
                    context.Database.ExecuteSqlCommand(dp);
                }
                catch (Exception ex)
                {
                    context.Database.ExecuteSqlCommand(SqlQueryBuilder.DropTable(tableInfo.FullTempOutputTableName));
                    throw;
                }
            }
            catch (Exception ex)
            {
                context.Database.ExecuteSqlCommand(SqlQueryBuilder.DropTable(tableInfo.FullTempTableName));
                throw;
            }
        }

        public static async Task MergeAsync<T>(DbContext context, IList<T> entities, TableInfo tableInfo, OperationType operationType, Action<decimal> progress) where T : class
        {
            tableInfo.InsertToTempTable = true;
            await tableInfo.CheckHasIdentityAsync(context).ConfigureAwait(false);

            await context.Database.ExecuteSqlCommandAsync(SqlQueryBuilder.CreateTableCopy(tableInfo.FullTableName, tableInfo.FullTempTableName)).ConfigureAwait(false);
            if (tableInfo.BulkConfig.SetOutputIdentity && tableInfo.HasIdentity)
            {
                await context.Database.ExecuteSqlCommandAsync(SqlQueryBuilder.CreateTableCopy(tableInfo.FullTableName, tableInfo.FullTempOutputTableName)).ConfigureAwait(false);
            }
            try
            {
                await InsertAsync(context, entities, tableInfo, progress).ConfigureAwait(false);
                await context.Database.ExecuteSqlCommandAsync(SqlQueryBuilder.MergeTable(tableInfo, operationType)).ConfigureAwait(false);
                await context.Database.ExecuteSqlCommandAsync(SqlQueryBuilder.DropTable(tableInfo.FullTempTableName)).ConfigureAwait(false);

                if (tableInfo.BulkConfig.SetOutputIdentity && tableInfo.HasIdentity)
                {
                    await tableInfo.UpdateOutputIdentityAsync(context, entities).ConfigureAwait(false);
                    await context.Database.ExecuteSqlCommandAsync(SqlQueryBuilder.DropTable(tableInfo.FullTempOutputTableName)).ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                if (tableInfo.BulkConfig.SetOutputIdentity && tableInfo.HasIdentity)
                {
                    await context.Database.ExecuteSqlCommandAsync(SqlQueryBuilder.DropTable(tableInfo.FullTempOutputTableName)).ConfigureAwait(false);
                }
                await context.Database.ExecuteSqlCommandAsync(SqlQueryBuilder.DropTable(tableInfo.FullTempTableName)).ConfigureAwait(false);
                throw;
            }
        }

        internal static SqlConnection OpenAndGetSqlConnection(DbContext context)
        {
            if (context.Database.GetDbConnection().State != ConnectionState.Open)
            {
                context.Database.GetDbConnection().Open();
            }
            return context.Database.GetDbConnection() as SqlConnection;
        }

        internal static async Task<SqlConnection> OpenAndGetSqlConnectionAsync(DbContext context)
        {
            if (context.Database.GetDbConnection().State != ConnectionState.Open)
            {
                await context.Database.GetDbConnection().OpenAsync().ConfigureAwait(false);
            }
            return context.Database.GetDbConnection() as SqlConnection;
        }

        private static SqlBulkCopy GetSqlBulkCopy(SqlConnection sqlConnection, DbContextTransaction transaction, bool keepIdentity = false)
        {
            if (transaction == null)
            {
                return keepIdentity ? new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.KeepIdentity, null) : new SqlBulkCopy(sqlConnection);
            }

            var sqlTransaction = (SqlTransaction)transaction.GetDbTransaction();
            return keepIdentity ? new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.KeepIdentity, sqlTransaction) : new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, sqlTransaction);
        }
    }
}
