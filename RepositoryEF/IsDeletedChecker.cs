﻿using BaseEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryEF
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
