using System;
using System.Linq;
using System.Linq.Expressions;
using BaseEntities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace RepositoryEFCore
{
    //TODO move isDeleted checking in decorator with update expression tree
    class IsDeletedChecker
    {
        public IQueryable<T> Select<T>(Expression<Func<T, bool>> predicate, DbContext dataContext) where T : class, IIsDeleted
        {
            return dataContext.Set<T>().Where(x => !x.IsDeleted).Where(predicate);
        }
    }
}
