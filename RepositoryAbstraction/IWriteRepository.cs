using BaseEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RepositoryAbstraction
{
    public interface IWriteRepository<T, TKey> where T: class, new() where TKey: struct
    {
        void Add(params T[] entities);

        void Add(IEnumerable<T> entities);
        void BulkInsert(IEnumerable<T> entityCollection, int? batchSize = 50000, int? commandTimeout = 300,
    BulkCopyOptions? bulkCopyOptions = BulkCopyOptions.Default);

        int Update(Expression<Func<T, bool>> getExpression, Expression<Func<T, T>> updateExpression);

        int Update(IQueryable<T> query, Expression<Func<T, T>> updateExpression);

        int SaveChanges();

        int Delete(T entity);

        int Delete(IEnumerable<T> entity);
        int Delete(IQueryable<T> entity);

        int Delete(Expression<Func<T, bool>> expression);

        int DeleteById(List<TKey> idCollection);


    }
}
