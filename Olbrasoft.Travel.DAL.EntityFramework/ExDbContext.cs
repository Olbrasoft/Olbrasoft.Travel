using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
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
        public static string GetPropertyName<T>(System.Linq.Expressions.Expression<Func<T, object>> property)
        {
            System.Linq.Expressions.LambdaExpression lambda = (System.Linq.Expressions.LambdaExpression)property;
            System.Linq.Expressions.MemberExpression memberExpression;

            if (lambda.Body is System.Linq.Expressions.UnaryExpression)
            {
                System.Linq.Expressions.UnaryExpression unaryExpression = (System.Linq.Expressions.UnaryExpression)(lambda.Body);
                memberExpression = (System.Linq.Expressions.MemberExpression)(unaryExpression.Operand);
            }
            else
            {
                memberExpression = (System.Linq.Expressions.MemberExpression)(lambda.Body);
            }

            return ((PropertyInfo)memberExpression.Member).Name;
        }


        public static void BulkUpdate<T>(this DbContext context, IEnumerable<T> entities, Action<EventArgs> onSaved, params Expression<Func<T,object>>[] ignoreProperties) where T : class
        {
            var batchesToUpdate = SplitList(entities);
            
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


        public static void  BulkInsert<T>(this DbContext context, IEnumerable<T> entities, Action<EventArgs> onSaved) where T : class
        {
            var batchesToInsert = SplitList(entities);

            foreach (var batch in batchesToInsert)
            {

               context.BulkInsert(batch,
                    new BulkConfig
                    {
                        BatchSize = 45000,
                        BulkCopyTimeout = 480,
                        IgnoreColumns = new HashSet<string>(new[] {"DateAndTimeOfCreation"})
                    });
                
                onSaved(EventArgs.Empty);
            }
        }

        

        public static IEnumerable<List<T>> SplitList<T>(IEnumerable<T> locations, int nSize = 90000)
        {
            var result = locations.ToList();
            for (var i = 0; i < result.Count; i += nSize)
            {
                yield return result.GetRange(i, Math.Min(nSize, result.Count - i));
            }
        }

    }
}