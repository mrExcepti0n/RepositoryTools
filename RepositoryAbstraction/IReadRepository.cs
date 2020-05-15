using BaseEntities.Interfaces;
using BaseEntities.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RepositoryAbstraction
{
    public interface IReadRepository<T, TKey> : IDisposable where T: IIdentityEntity<TKey> where TKey: struct
    {
        T SingleOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        bool Any(params Expression<Func<T, object>>[] includes);
        IEnumerable<T> Select(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        IEnumerable<T> Select(IEnumerable<TKey> collection, params Expression<Func<T, object>>[] includes);
        IEnumerable<T> Select(ISpecification specification, params Expression<Func<T, object>>[] includes);

        IEnumerable<T> Select(params Expression<Func<T, object>>[] includes);

        IQueryable<T> QueryableSelect(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        IQueryable<T> QueryableSelect(params Expression<Func<T, object>>[] includes);

        IQueryable<T> QueryableSelect(ISpecification specification, params Expression<Func<T, object>>[] includes);

        MKey? Max<MKey>(Expression<Func<T, MKey>> selector) where MKey : struct;
        MKey? Max<MKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, MKey>> selector) where MKey : struct;
    }
}
