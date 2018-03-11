using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Olbrasoft.EntityFramework.Bulk;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public static class ExDbContext
    {
        public static void BulkUpdate<T>(this DbContext context, IEnumerable<T> entities, Action<EventArgs> onSaved) where T : class
        {
            var batchesToUpdate = SplitList(entities);

            foreach (var batch in batchesToUpdate)
            {
              context.BulkUpdate(batch, new BulkConfig()
                {
                    BatchSize = 45000,
                    BulkCopyTimeout = 480,
                    IgnoreColumns = new HashSet<string>(new[] { "DateAndTimeOfCreation" }),
                    IgnoreColumnsUpdate = new HashSet<string>(new[] { "CreatorId" })
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