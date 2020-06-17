using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryAbstraction;
using RepositoryAbstraction.Tables;
using RepositoryEF.Extensions;

namespace RepositoryEF
{
    public class TemporaryTableRepository : ITemporaryTableRepository
    {
        public TemporaryTableRepository(DbContext dbContext) 
        {
            _dataContext = dbContext;
        }

        private bool _isDisposed;
        private readonly DbContext _dataContext;
        protected DbContext DataContext => _dataContext;

        private readonly ConcurrentBag<string> _createdTemporaryTableNames = new ConcurrentBag<string>();

        public IQueryable<T> BulkInsertInTempTable<T>(IEnumerable<T> entityCollection) where T : class
        {
            TemporaryTableGeneration.CreateTemporaryTable<T>(DataContext);
            BulkInsertHelper.BulkInsertWithTransaction(entityCollection, DataContext);

            _createdTemporaryTableNames.Add("#" + typeof(T).Name);

            return DataContext.Set<T>();
        }

        public IQueryable<TemporaryIntIdentity> BulkInsertInTempTable(IEnumerable<int> entityCollection)
        {
            return BulkInsertInTempTable(entityCollection.Select(id => new TemporaryIntIdentity(id)));
        }

        public IQueryable<TemporaryGuidIdentity> BulkInsertInTempTable(IEnumerable<Guid> entityCollection)
        {
            return BulkInsertInTempTable(entityCollection.Select(id => new TemporaryGuidIdentity(id)));
        }

        public IQueryable<TemporaryStringIdentity> BulkInsertInTempTable(IEnumerable<string> entityCollection)
        {
            return BulkInsertInTempTable(entityCollection.Select(id => new TemporaryStringIdentity(id)));
        }

        public void Dispose()
        {
            if (!_isDisposed && DataContext != null)
            {
                CleanUp();
                DataContext.Dispose();
                _isDisposed = true;
            }
        }

        private void CleanUp()
        {
            if (_createdTemporaryTableNames.Any())
            {
                if (DataContext.Database.Connection.State == ConnectionState.Closed)
                {
                    DataContext.Database.Connection.Open();
                }
                foreach (var tmpName in _createdTemporaryTableNames.Distinct().ToList())
                {
                    string sqlCommand = $"IF OBJECT_ID('tempdb..{tmpName}') IS NOT NULL BEGIN DROP TABLE {tmpName} END";
                    ExecuteSqlCommand(sqlCommand);
                }
                DataContext.Database.Connection.Close();
            }
        }

        private int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return DataContext.Database.ExecuteSqlCommand(sql, parameters);
        }

    }
}
