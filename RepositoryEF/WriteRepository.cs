using BaseEntities.Interfaces;
using RepositoryAbstraction;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace RepositoryEF
{
    public class WriteRepository<T, TKey> : WriteBaseRepository<T, TKey> where T : class, IIdentityEntity<TKey>, new() where TKey : struct
    {
        public WriteRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private DbContext _dbContext;
        private DbSet<T> _dbSet => _dbContext.Set<T>();

        public override void BulkInsert(IEnumerable<T> entityCollection, int? batchSize = 50000, int? commandTimeout = 300, BulkCopyOptions? bulkCopyOptions = BulkCopyOptions.Default)
        {
            throw new NotImplementedException();
        }

        public override int Delete(IEnumerable<T> entity)
        {
            throw new NotImplementedException();
        }

        public override int Delete(IQueryable<T> query)
        {
            return query.Delete();
        }

        public override int Delete(Expression<Func<T, bool>> expression)
        {
            return Delete(_dbSet.Where(expression));
        }

        public override int DeleteById(List<TKey> idCollection)
        {
            throw new NotImplementedException();
        }

        public override int SaveChanges()
        {
           return _dbContext.SaveChanges();
        }

        public override int Update(Expression<Func<T, bool>> getExpression, Expression<Func<T, T>> updateExpression)
        {
            return Update(_dbSet.Where(getExpression), updateExpression);
        }

        public override void Add(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public override int Update(IQueryable<T> query, Expression<Func<T, T>> updateExpression)
        {
            return query.Update(updateExpression);
        }
    }
}
