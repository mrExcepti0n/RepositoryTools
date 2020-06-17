using System;
using EntityFramework.BulkInsert;
using EntityFramework.BulkInsert.Extensions;
using EntityFramework.BulkInsert.Helpers;
using EntityFramework.BulkInsert.Providers;
using EntityFramework.MappingAPI;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;

namespace RepositoryEF.Extensions
{
    public static class BulkInsertHelper
    {
        public static void BulkInsertWithTransaction<T>(IEnumerable<T> entityCollection, DbContext context)
        {
            var provider = new EfSqlBulkInsertProviderWithMappedDataReader();
            provider.SetContext(context);

            SqlTransaction transaction = context.Database.CurrentTransaction?.UnderlyingTransaction as SqlTransaction;

            using (var mappedDataReader = new MappedDataReader<T>(entityCollection, provider))
            {
                var databaseConnection = context.Database.Connection as SqlConnection ?? throw new InvalidOperationException("Can't cast  Context.Database.Connection to type 'SqlConnection'");

                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(databaseConnection, SqlBulkCopyOptions.Default, transaction))
                {
                    sqlBulkCopy.DestinationTableName = $"[{mappedDataReader.SchemaName}].[{mappedDataReader.TableName}]";
                    using (var enumerator = mappedDataReader.Cols.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            KeyValuePair<int, IPropertyMap> current = enumerator.Current;
                            sqlBulkCopy.ColumnMappings.Add(current.Value.ColumnName, current.Value.ColumnName);
                        }
                    }
                    sqlBulkCopy.WriteToServer(mappedDataReader);
                }
            }
        }

        public static void BulkInsert<T>(this DbContext context, IEnumerable<T> entities, IDbTransaction transaction, BulkInsertOptions options)
        {
            ProviderFactory.Get(context).Run(entities, transaction, options);
        }
    }
}
