﻿using BaseEntities.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RepositoryAbstraction;

namespace RepositoryEFCore
{
    public class ReadRepository<T, TKey> : ReadBaseRepository<T, TKey> where T : class, IIdentityEntity<TKey> where TKey : struct
    {
        protected ReadRepository(DbContext dbContext)
        {
            _dataContext = dbContext;
        }

        private readonly DbContext _dataContext;
        public DbContext DataContext => _dataContext;

        private bool _isIncludeDeleted = true;

        public override void Dispose()
        {
            DataContext.Dispose();
        }


        //TODO nested includes don't work in ef core (should use ThenInclude)
        public override IQueryable<T> QueryableSelect(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> result;

            if (_isIncludeDeleted && typeof(IIsDeleted).IsAssignableFrom(typeof(T)))
            {
                result = ((dynamic)new IsDeletedChecker()).Select(predicate, DataContext);
            }
            else
            {
                result = DataContext.Set<T>().Where(predicate);
            }

            if (includes != null)
            {
                result = includes.Aggregate(result,
                    (current, include) => current.Include(include));
            }
            return result;
        }
    }
}
