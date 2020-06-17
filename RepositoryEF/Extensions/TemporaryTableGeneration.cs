using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryEF.Extensions
{
    public static class TemporaryTableGeneration
    {

        public static void CreateTemporaryTable<TTemporaryEntity>(DbContext dbContext)
            where TTemporaryEntity : class
        {
            IDictionary<string, EdmProperty> temporarySnapshotColumns = dbContext.GetEntityPropertyMappings<TTemporaryEntity>().ToDictionary(p => p.Property.Name, p => p.Column);

            if (temporarySnapshotColumns.Any())
            {
                string temporarySnapshotTableName = "#" + typeof(TTemporaryEntity).Name;

                var properties = typeof(TTemporaryEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(prop => prop.Name).ToList();


                StringBuilder temporarySnapshotCreateColumnsListBuilder = new StringBuilder();
                string keyName = (temporarySnapshotColumns.FirstOrDefault().Value.DeclaringType as EntityType)?.KeyMembers.FirstOrDefault()?.Name;

                foreach (var propertyName in properties)
                {
                    EdmProperty temporarySnapshotColumn = temporarySnapshotColumns[propertyName];
                    temporarySnapshotCreateColumnsListBuilder.Append(GetTemporarySnapshotColumnCreateSql(temporarySnapshotColumn, keyName));
                }
                temporarySnapshotCreateColumnsListBuilder.Length -= 1;

                string temporarySnapshotCreateSqlCommand = String.Format("IF OBJECT_ID('tempdb..{0}') IS NOT NULL BEGIN DROP TABLE {0} END{1}CREATE TABLE {0} ({2})",
                    temporarySnapshotTableName, Environment.NewLine, temporarySnapshotCreateColumnsListBuilder);

                if (dbContext.Database.Connection.State == ConnectionState.Closed)
                {
                    dbContext.Database.Connection.Open();
                }

                dbContext.Database.ExecuteSqlCommand(temporarySnapshotCreateSqlCommand);
            }
        }


        private static string GetTemporarySnapshotColumnCreateSql(EdmProperty temporarySnapshotColumn, string keyName)
        {
            string typeNameUpperCase = temporarySnapshotColumn.TypeName.ToUpperInvariant();
            string temporarySnapshotColumnCreateSqlSuffix = temporarySnapshotColumn.Nullable ? "" : " NOT NULL";
            string result;
            switch (typeNameUpperCase)
            {
                case "NUMERIC":
                    result = $"[{temporarySnapshotColumn.Name}] NUMERIC({temporarySnapshotColumn.Precision},{temporarySnapshotColumn.Scale}){temporarySnapshotColumnCreateSqlSuffix}";
                    break;
                case "NVARCHAR":
                case "VARCHAR":
                    result = $"[{temporarySnapshotColumn.Name}] {typeNameUpperCase}({temporarySnapshotColumn.MaxLength}){temporarySnapshotColumnCreateSqlSuffix}";
                    break;
                default:
                    result = $"[{temporarySnapshotColumn.Name}] {typeNameUpperCase}{temporarySnapshotColumnCreateSqlSuffix}";
                    break;
            }
            if (temporarySnapshotColumn.Name == keyName)
            {
                result += " PRIMARY KEY CLUSTERED";
            }

            result += ",";
            return result;
        }
    }
}
