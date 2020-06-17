using RepositoryAbstraction.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryAbstraction
{
    public interface ITemporaryTableRepository
    {

        IQueryable<T> BulkInsertInTempTable<T>(IEnumerable<T> entityCollection) where T : class;

        IQueryable<TemporaryIntIdentity> BulkInsertInTempTable(IEnumerable<int> entityCollection);
        IQueryable<TemporaryGuidIdentity> BulkInsertInTempTable(IEnumerable<Guid> entityCollection);
        IQueryable<TemporaryStringIdentity> BulkInsertInTempTable(IEnumerable<string> entityCollection);
    }
}
