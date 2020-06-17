using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BaseEntities.Interfaces;
using EFCore.BulkExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RepositoryAbstraction;

namespace RepositoryEFCore
{
    public class WriteRepository<T, TKey> : WriteBaseRepository<T, TKey> where T : class, IIdentityEntity<TKey>, new() where TKey : struct
    {
        public WriteRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly DbContext _dbContext;
        internal DbSet<T> DbSet => _dbContext.Set<T>();


        //TODO needs check transaction 
        public override void BulkInsert(IEnumerable<T> entityCollection, int? batchSize = 50000, int? commandTimeout = 300, BulkCopyOptions? bulkCopyOptions = BulkCopyOptions.Default)
        {
            var collection = entityCollection as T[] ?? entityCollection.ToArray();

            _dbContext.BulkInsert(collection, config =>
            {
                config.SetOutputIdentity = true;
                config.BulkCopyTimeout = commandTimeout;
                config.BatchSize = batchSize ?? config.BatchSize;
                if (bulkCopyOptions != null) 
                    config.SqlBulkCopyOptions = (SqlBulkCopyOptions) bulkCopyOptions;
            });
        }


        public override int Delete(IEnumerable<T> entities)
        {
            var collection = entities.ToList();
            DbSet.RemoveRange(collection);
            return collection.Count();
        }

        public override int Delete(Expression<Func<T, bool>> expression)
        {
            return Delete(DbSet.Where(expression));
        }

        public override int DeleteById(List<TKey> idCollection)
        {
            return Delete(entity => idCollection.Contains(entity.Id));
        }

        public override int SaveChanges()
        {
           return _dbContext.SaveChanges();
        }

        public override int Update(Expression<Func<T, bool>> getExpression, Expression<Func<T, T>> updateExpression)
        {
            IQueryable<T> query = DbSet.Where(getExpression);

            return query.BatchUpdate(updateExpression);
        }

        public override void Add(IEnumerable<T> entities)
        {
            DbSet.AddRange(entities);
        }
    }
}
