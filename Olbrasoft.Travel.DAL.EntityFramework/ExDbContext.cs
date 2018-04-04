using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Olbrasoft.EntityFramework.Bulk;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public static class ExDbContext
    {

        /// <summary>
        ///     Returns the name of the specified property of the specified type.
        /// </summary>
        /// <typeparam name="T">
        ///     The type the property is a member of.
        /// </typeparam>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <returns>
        ///     The property name.
        /// </returns>
        public static string GetPropertyName<T>(Expression<Func<T, object>> property)
        {
            var lambda = (LambdaExpression)property;
            MemberExpression memberExpression;

            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)(lambda.Body);

                memberExpression = (MemberExpression)(unaryExpression.Operand);
            }
            else
            {
                memberExpression = (MemberExpression)(lambda.Body);
            }

            return ((PropertyInfo)memberExpression.Member).Name;
        }


        public static void BulkUpdate<T>(this DbContext context, IEnumerable<T> entities, Action<EventArgs> onSaved, params Expression<Func<T,object>>[] ignoreProperties) where T : class
        {
            var batchesToUpdate =entities.SplitToEanumerableOfList(90000);
            
            var ignoreColumnsUpdate = new HashSet<string>(ignoreProperties.Select(GetPropertyName));

            const string creatorColumn = "CreatorId";
            if (!ignoreColumnsUpdate.Contains(creatorColumn)) ignoreColumnsUpdate.Add(creatorColumn);
      

            foreach (var batch in batchesToUpdate)
            {
              context.BulkUpdate(batch, new BulkConfig()
                {
                    BatchSize = 45000,
                    BulkCopyTimeout = 480,
                    IgnoreColumns = new HashSet<string>(new[] { "DateAndTimeOfCreation" }),
                    IgnoreColumnsUpdate = ignoreColumnsUpdate
                });
                onSaved(EventArgs.Empty);
            }
        }


        public static void  BulkInsert<T>(this DbContext context, IEnumerable<T> entities, Action<EventArgs> onSaved, params Expression<Func<T, object>>[] ignoreProperties) where T : class
        {
            var batchesToInsert = entities.SplitToEanumerableOfList(90000);
            var ignoreColmnsInsert = new HashSet<string>(ignoreProperties.Select(GetPropertyName));
            
            const string createDateColumn = "DateAndTimeOfCreation";
            if (!ignoreColmnsInsert.Contains(createDateColumn)) ignoreColmnsInsert.Add(createDateColumn);
            
            foreach (var batch in batchesToInsert)
            {

               context.BulkInsert(batch,
                    new BulkConfig
                    {
                        BatchSize = 45000,
                        BulkCopyTimeout = 480,
                        IgnoreColumns = ignoreColmnsInsert
                    });
                
                onSaved(EventArgs.Empty);
            }
        }

        public static void BulkDev<T>(this DbContext context, IEnumerable<T> entities, BulkConfig config) where T : class
        {
            context.BulkInsert(entities.ToArray(),config);

        }

    }
}