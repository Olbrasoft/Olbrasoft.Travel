using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using FastMember;

namespace Olbrasoft.EntityFramework.Bulk
{
    public class TableInfo
    {
        public string Schema { get; set; }
        public string SchemaFormated => Schema != null ? Schema + "." : "";
        public string TableName { get; set; }
        public string FullTableName => $"{SchemaFormated}[{TableName}]";
        public List<string> PrimaryKeys { get; set; }
        public bool HasSinglePrimaryKey { get; set; }

        protected string TempDBPrefix => BulkConfig.UseTempDB ? "#" : "";
        public string TempTableSufix { get; set; }
        public string TempTableName => $"{TableName}{TempTableSufix}";
        public string FullTempTableName => $"{SchemaFormated}[{TempDBPrefix}{TempTableName}]";
        public string FullTempOutputTableName => $"{SchemaFormated}[{TempDBPrefix}{TempTableName}Output]";

        public bool InsertToTempTable { get; set; }
        public bool HasIdentity { get; set; }
        public int NumberOfEntities { get; set; }

        public BulkConfig BulkConfig { get; set; }
        public Dictionary<string, string> PropertyColumnNamesDict { get; set; } = new Dictionary<string, string>();
        public HashSet<string> ShadowProperties { get; set; } = new HashSet<string>();

        public static TableInfo CreateInstance<T>(DbContext context, IList<T> entities, OperationType operationType, BulkConfig bulkConfig)
        {
            var tableInfo = new TableInfo();
            var isDeleteOperation = operationType == OperationType.Delete;
            tableInfo.NumberOfEntities = entities.Count;
            tableInfo.LoadData<T>(context, isDeleteOperation);
            tableInfo.BulkConfig = bulkConfig ?? new BulkConfig();
            return tableInfo;
        }

        public void LoadData<T>(DbContext context, bool loadOnlyPKColumn)
        {


            var mapping = EfMappingFactory.GetMappingsForContext(context);
            TypeMapping typeMapping=null;
            if (mapping.TypeMappings.ContainsKey(typeof(T)))
            {
                typeMapping = mapping.TypeMappings[typeof(T)];
            }
            else
            {
                foreach (var mappingTypeMapping in mapping.TypeMappings)
                {
                    foreach (var valueTableMapping in mappingTypeMapping.Value.TableMappings.Where(p=>p.TPHConfiguration!=null))
                    {
                        if (valueTableMapping.TPHConfiguration.Mappings.ContainsKey(typeof(T)))
                        {
                            typeMapping = mappingTypeMapping.Value;
                        }
                    }

                }
            }
            

            var tableMapping = typeMapping.TableMappings.First();

            var prop = tableMapping.PropertyMappings
                .Select(p => new ColumnMapping { NameInDatabase = p.ColumnName, NameOnObject = p.PropertyName }).ToList();


            if (tableMapping.TPHConfiguration != null)
            {
                prop.Add(new ColumnMapping
                {
                    NameInDatabase = tableMapping.TPHConfiguration.ColumnName,
                    StaticValue = tableMapping.TPHConfiguration.Mappings[typeof(T)]
                });
            }


            Schema = tableMapping.Schema;
            TableName = tableMapping.TableName;
            

            TempTableSufix = Guid.NewGuid().ToString().Substring(0, 8); // 8 chars of Guid as tableNameSufix to avoid same name collision with other tables

            PrimaryKeys = tableMapping.PropertyMappings.Where(p => p.IsPrimaryKey).Select(p => p.PropertyName)
                .ToList();
                
                //GetEntitySet(typeof(T) ,context).ElementType.KeyMembers.Select(e=>e.Name).ToList();
            
            HasSinglePrimaryKey = PrimaryKeys.Count == 1;
            
            if (loadOnlyPKColumn)
            {
                        PropertyColumnNamesDict = tableMapping.PropertyMappings.Where(p => p.IsPrimaryKey)
                        .ToDictionary(key => key.PropertyName, name => name.PropertyName);
             
            }
            else
            {


                PropertyColumnNamesDict = tableMapping.PropertyMappings
                    .ToDictionary(key => key.PropertyName, name => name.PropertyName);
                
                ShadowProperties = new HashSet<string>();
            }
        }
        

        public void CheckHasIdentity(DbContext context)
        {
            int hasIdentity = 0;
            if (HasSinglePrimaryKey)
            {
                var sqlConnection = context.Database.GetDbConnection();
                var currentTransaction = context.Database.CurrentTransaction;
                try
                {
                    if (currentTransaction == null)
                    {
                        if (sqlConnection.State != ConnectionState.Open)
                            sqlConnection.Open();
                    }
                    using (var command = sqlConnection.CreateCommand())
                    {
                        if (currentTransaction != null)
                            command.Transaction = currentTransaction.GetDbTransaction();
                        command.CommandText = SqlQueryBuilder.SelectIsIdentity(FullTableName, PropertyColumnNamesDict[PrimaryKeys[0]]);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    hasIdentity = reader[0] == DBNull.Value ? 0 : (int)reader[0];
                                }
                            }
                        }
                    }
                }
                finally
                {
                    if (currentTransaction == null)
                        sqlConnection.Close();
                }
            }
            HasIdentity = hasIdentity == 1;
        }

        public async Task CheckHasIdentityAsync(DbContext context)
        {
            int hasIdentity = 0;
            if (HasSinglePrimaryKey)
            {
                var sqlConnection = context.Database.GetDbConnection();
                var currentTransaction = context.Database.CurrentTransaction;
                try
                {
                    if (currentTransaction == null)
                    {
                        if (sqlConnection.State != ConnectionState.Open)
                            await sqlConnection.OpenAsync().ConfigureAwait(false);
                    }
                    using (var command = sqlConnection.CreateCommand())
                    {
                        if (currentTransaction != null)
                            command.Transaction = currentTransaction.GetDbTransaction();
                        command.CommandText = SqlQueryBuilder.SelectIsIdentity(FullTableName, PropertyColumnNamesDict[PrimaryKeys[0]]);
                        using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                        {
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync().ConfigureAwait(false))
                                {
                                    hasIdentity = (int)reader[0];
                                }
                            }
                        }
                    }
                }
                finally
                {
                    if (currentTransaction == null)
                    {
                        sqlConnection.Close();
                    }
                }
            }
            HasIdentity = hasIdentity == 1;
        }

        public void SetSqlBulkCopyConfig<T>(SqlBulkCopy sqlBulkCopy, IList<T> entities, Action<decimal> progress)
        {
            sqlBulkCopy.DestinationTableName = this.InsertToTempTable ? this.FullTempTableName : this.FullTableName;
            sqlBulkCopy.BatchSize = BulkConfig.BatchSize;
            sqlBulkCopy.NotifyAfter = BulkConfig.NotifyAfter ?? BulkConfig.BatchSize;
            sqlBulkCopy.SqlRowsCopied += (sender, e) => {
                progress?.Invoke((decimal)(e.RowsCopied * 10000 / entities.Count) / 10000); // round to 4 decimal places
            };
            sqlBulkCopy.BulkCopyTimeout = BulkConfig.BulkCopyTimeout ?? sqlBulkCopy.BulkCopyTimeout;
            sqlBulkCopy.EnableStreaming = BulkConfig.EnableStreaming;

            foreach (var element in this.PropertyColumnNamesDict)
            {
                sqlBulkCopy.ColumnMappings.Add(element.Key, element.Value);
            }
        }

        public void UpdateOutputIdentity<T>(DbContext context, IList<T> entities) where T : class
        {
            if (this.HasSinglePrimaryKey)
            {
                var entitiesWithOutputIdentity = QueryOutputTable<T>(context).ToList();
                UpdateEntitiesIdentity(entities, entitiesWithOutputIdentity);
            }
        }

        public async Task UpdateOutputIdentityAsync<T>(DbContext context, IList<T> entities) where T : class
        {
            if (this.HasSinglePrimaryKey)
            {
                var entitiesWithOutputIdentity = await QueryOutputTable<T>(context).ToListAsync().ConfigureAwait(false);
                UpdateEntitiesIdentity(entities, entitiesWithOutputIdentity);
            }
        }

        protected IQueryable<T> QueryOutputTable<T>(DbContext context) where T : class
        {
             
            var query = context.Set<T>().SqlQuery( SqlQueryBuilder.SelectFromTable(this.FullTempOutputTableName)).AsQueryable();
                
            return query.OrderBy(PrimaryKeys[0]);

        }

        protected void UpdateEntitiesIdentity<T>(IList<T> entities, IList<T> entitiesWithOutputIdentity)
        {
            if (this.BulkConfig.PreserveInsertOrder) // Updates PK in entityList
            {
                var accessor = TypeAccessor.Create(typeof(T));
                for (int i = 0; i < this.NumberOfEntities; i++)
                    accessor[entities[i], this.PrimaryKeys[0]] = accessor[entitiesWithOutputIdentity[i], this.PrimaryKeys[0]];
            }
            else // Clears entityList and then refill it with loaded entites from Db
            {
                entities.Clear();
                ((List<T>)entities).AddRange(entitiesWithOutputIdentity);
            }
        }
        
        private static IQueryable<T> OrderBy<T>(IQueryable<T> source, string ordering)
        {
            Type entityType = typeof(T);
            PropertyInfo property = entityType.GetProperty(ordering);
            ParameterExpression parameter = Expression.Parameter(entityType);
            MemberExpression propertyAccess = Expression.MakeMemberAccess(parameter, property);
            LambdaExpression orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { entityType, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            var orderedQuery = source.Provider.CreateQuery<T>(resultExp);
            return orderedQuery;
        }
    }
}
