﻿using BaseEntities.Interfaces;
using RepositoryAbstraction;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using EntityFramework.BulkInsert.Extensions;
using RepositoryEF.Extensions;
using Z.EntityFramework.Plus;

namespace RepositoryEF
{
    public class WriteRepository<T, TKey> : WriteBaseRepository<T, TKey> where T : class, IIdentityEntity<TKey>, new() where TKey : struct
    {
        public WriteRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly DbContext _dbContext;
        internal DbSet<T> DbSet => _dbContext.Set<T>();

        public override void BulkInsert(IEnumerable<T> entityCollection, int? batchSize = 50000, int? commandTimeout = 300, BulkCopyOptions? bulkCopyOptions = BulkCopyOptions.Default)
        {
            var collection = entityCollection as T[] ?? entityCollection.ToArray();

            DbContextTransaction tran = _dbContext.Database.CurrentTransaction;

            SqlBulkCopyOptions sqlBulkCopyOptions = bulkCopyOptions == null
                ? SqlBulkCopyOptions.Default
                : (SqlBulkCopyOptions)(int)bulkCopyOptions.Value;

            var bulkInsertOptions = new BulkInsertOptions
            {
                BatchSize = batchSize ?? 5000,
                TimeOut = commandTimeout ?? 30,
                SqlBulkCopyOptions = sqlBulkCopyOptions
            };

            if (tran != null)
            {
                _dbContext.BulkInsert(collection, _dbContext.Database.CurrentTransaction.UnderlyingTransaction, bulkInsertOptions);
            }
            else
            {
                _dbContext.BulkInsert(collection, bulkInsertOptions);
            }
        }


        public override int Delete(IEnumerable<T> entities)
        {
            var removed = DbSet.RemoveRange(entities);

            return removed.Count();
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
            return DbSet.Where(getExpression).Update(updateExpression);
        }

        public override void Add(IEnumerable<T> entities)
        {
            DbSet.AddRange(entities);
        }
    }
}
