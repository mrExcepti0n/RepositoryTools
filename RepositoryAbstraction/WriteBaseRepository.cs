using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RepositoryAbstraction
{
    public abstract class WriteBaseRepository<T, TKey> : IWriteRepository<T, TKey> where T : class, new() where TKey : struct
    {
        public WriteBaseRepository()
        {
        }
     

        public void Add(params T[] entities)
        {
            Add(entities as IEnumerable<T>);
        }


        public abstract void Add(IEnumerable<T> entities);


        public abstract void BulkInsert(IEnumerable<T> entityCollection, int? batchSize = 50000, int? commandTimeout = 300, BulkCopyOptions? bulkCopyOptions = BulkCopyOptions.Default);

        public int Delete(T entity)
        {
            return Delete(new List<T> { entity });
        }

        public abstract int Delete(IEnumerable<T> entity);

        public abstract int Delete(IQueryable<T> entity);

        public abstract int Delete(Expression<Func<T, bool>> expression);

        public abstract int DeleteById(List<TKey> idCollection);

        public abstract int SaveChanges();

        public abstract int Update(Expression<Func<T, bool>> getExpression, Expression<Func<T, T>> updateExpression);

        public abstract int Update(IQueryable<T> query, Expression<Func<T, T>> updateExpression);
    }
}
