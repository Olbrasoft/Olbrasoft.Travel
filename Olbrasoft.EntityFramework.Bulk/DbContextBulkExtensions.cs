﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Olbrasoft.EntityFramework.Bulk
{
    public static class DbContextBulkExtensions
    {
       
        public static void BulkInsert<T>(this DbContext context, IList<T> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where T : class
        {
            DbContextBulkTransaction.Execute(context, entities, OperationType.Insert,  bulkConfig, progress);
        }
        
        public static void BulkInsertOrUpdate<T>(this DbContext context, IList<T> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where T : class
        {
            DbContextBulkTransaction.Execute(context, entities, OperationType.InsertOrUpdate,  bulkConfig, progress);
        }
        

        public static void BulkUpdate<T>(this DbContext context, IList<T> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where T : class
        {
            DbContextBulkTransaction.Execute(context, entities, OperationType.Update,  bulkConfig, progress);
        }

        public static void BulkDelete<T>(this DbContext context, IList<T> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where T : class
        {
            DbContextBulkTransaction.Execute(context, entities, OperationType.Delete,  bulkConfig, progress);
        }

        // Async methods

        public static Task BulkInsertAsync<T>(this DbContext context, IList<T> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where T : class
        {
            return DbContextBulkTransaction.ExecuteAsync(context, entities, OperationType.Insert,  bulkConfig, progress);
        }

        public static Task BulkInsertOrUpdateAsync<T>(this DbContext context, IList<T> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where T : class
        {
            return DbContextBulkTransaction.ExecuteAsync(context, entities, OperationType.InsertOrUpdate,  bulkConfig, progress);
        }

        public static Task BulkUpdateAsync<T>(this DbContext context, IList<T> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where T : class
        {
            return DbContextBulkTransaction.ExecuteAsync(context, entities, OperationType.Update,  bulkConfig, progress);
        }

        public static Task BulkDeleteAsync<T>(this DbContext context, IList<T> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where T : class
        {
            return DbContextBulkTransaction.ExecuteAsync(context, entities, OperationType.Delete,  bulkConfig, progress);
        }


        public static void BulkInsert<T>(this DbContext context, IEnumerable<T> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where T : class
        {
            BulkInsert(context, entities.ToList(), bulkConfig, progress);
        }

        public static void BulkUpdate<T>(this DbContext context, IEnumerable<T> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where T : class
        {
            BulkUpdate(context, entities.ToList(), bulkConfig, progress);
        }

        public static void BulkInsertOrUpdate<T>(this DbContext context, IEnumerable<T> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where T : class
        {
            BulkInsertOrUpdate(context, entities.ToList(), bulkConfig, progress);
        }

        public static void BulkDelete<T>(this DbContext context, IEnumerable<T> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null) where T : class
        {
            BulkDelete(context, entities.ToList(), bulkConfig, progress);
        }
    }
}
