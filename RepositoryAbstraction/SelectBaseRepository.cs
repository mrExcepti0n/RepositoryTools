using BaseEntities.Interfaces;
using BaseEntities.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RepositoryAbstraction
{
    public abstract class SelectBaseRepository<T, TKey> : ISelectRepository<T, TKey> where T : class, IIdentityEntity<TKey> where TKey : struct
    {
        public IQueryable<T> QueryableSelect(params Expression<Func<T, object>>[] includes)
        {
            return QueryableSelect(pred => true, includes);
        }

        public abstract IQueryable<T> QueryableSelect(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
            

        public IEnumerable<T> Select(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return QueryableSelect(predicate, includes).ToList();
        }

        public IEnumerable<T> Select(params Expression<Func<T, object>>[] includes)
        {
            return Select((p) => true, includes).ToList();
        }

        public IEnumerable<T> Select(ISpecification specification, params Expression<Func<T, object>>[] includes)
        {
            return Select(specification.GetSatisfiedExpression<T>(), includes);
        }

        public IQueryable<T> QueryableSelect(ISpecification specification, params Expression<Func<T, object>>[] includes)
        {
            return QueryableSelect(specification.GetSatisfiedExpression<T>(), includes);
        }

        public IEnumerable<dynamic> DynamicSelect(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> selector, params Expression<Func<T, object>>[] includes)
        {
            return QueryableSelect(predicate, includes).Select(selector).ToList();
        }

        public IEnumerable<T> Select(IEnumerable<TKey> collection, params Expression<Func<T, object>>[] includes)
        {
            Expression<Func<T, bool>> expression = el => collection.Contains(el.Id);
            return Select(expression, includes);
        }

        public bool Any(params Expression<Func<T, object>>[] includes)
        {
            return QueryableSelect(includes).Any();
        }

        public MKey? Max<MKey>(Expression<Func<T, MKey>> selector) where MKey : struct
        {
            return Max(x => true, selector);
        }

        public MKey? Max<MKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, MKey>> selector) where MKey : struct
        {
            if (!Any())
            {
                return null;
            }

            return QueryableSelect(predicate).Max(selector);
        }



        public abstract void Dispose();

        public T SingleOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return QueryableSelect(predicate, includes).SingleOrDefault();
        }
    }
}
