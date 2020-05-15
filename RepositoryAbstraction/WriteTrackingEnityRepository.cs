using BaseEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RepositoryAbstraction
{
    public class WriteTrackingEnityRepository<T, TKey> : IWriteRepository<T, TKey> where T : class, ITable<TKey>,  new() where TKey : struct
    {
        public WriteTrackingEnityRepository(WriteBaseRepository<T, TKey> repository, IIdentityProvider identityProvider)
        {
            _repository = repository;
            _isPhysicalDelete = false;
            _identityProvider = identityProvider;
        }
        
        private IIdentityProvider _identityProvider;
        private WriteBaseRepository<T, TKey> _repository;
        private bool _isPhysicalDelete;

        private void ApplayUpdateDate(IEnumerable<IChangeDate> entityCollection)
        {
            var dateTimeNow = DateTime.Now;
            foreach (var element in entityCollection)
            {
                element.ChangeDate = dateTimeNow;
            }
        }

        private void ApplayWorkerChangers(IEnumerable<IChangedBy> entityCollection)
        {
            foreach (IChangedBy element in entityCollection)
            {
                element.ChangedBy = _identityProvider.User;
            }
        }

        private int SoftDelete(IEnumerable<T> entities)
        {
            ApplayUpdateDate(entities);
            ApplayWorkerChangers(entities);
            foreach (var element in entities)
            {
                element.IsDeleted = true;
            }

            return entities.Count();
        }


        public void Add(params T[] entities)
        {
            Add(entities as IEnumerable<T>);
        }

        public void Add(IEnumerable<T> entities)
        {
            ApplayUpdateDate(entities);
            ApplayWorkerChangers(entities);
            _repository.Add(entities);
        }

        public void BulkInsert(IEnumerable<T> entityCollection, int? batchSize = 50000, int? commandTimeout = 300, BulkCopyOptions? bulkCopyOptions = BulkCopyOptions.Default)
        {
            _repository.BulkInsert(entityCollection, batchSize, commandTimeout, bulkCopyOptions);
        }

        public int Delete(T entity)
        {            
            return Delete(new List<T> { entity });
        }

        public int Delete(IEnumerable<T> entities)
        {
            if (!_isPhysicalDelete)
            {
                return _repository.Delete(entities);
            }

            return SoftDelete(entities);
        }

        public int Delete(IQueryable<T> entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(Expression<Func<T, bool>> expression)
        {
            if (!_isPhysicalDelete)
            {
                return Update(expression, entity => new T { IsDeleted = true });
            }

            return _repository.Delete(expression);
        }

        public int DeleteById(List<TKey> idCollection)
        {
            throw new NotImplementedException();
        }

        public virtual int SaveChanges()
        {
            return _repository.SaveChanges();
        }

        public int Update(Expression<Func<T, bool>> getExpression, Expression<Func<T, T>> updateExpression)
        {
            var memberInitExpression = updateExpression.Body as MemberInitExpression;

            var bindings = memberInitExpression.Bindings.ToList();
            var dateTime = DateTime.Now;

            bindings.Add(Expression.Bind(typeof(T).GetMember("ChangedBy")[0], Expression.Constant(_identityProvider.User)));
            bindings.Add(Expression.Bind(typeof(T).GetMember("ChangeDate")[0], Expression.Constant(dateTime)));
            MemberInitExpression newMemberInitExpression = Expression.MemberInit(Expression.New(typeof(T)), bindings);
            var newUpdateExpression = Expression.Lambda(newMemberInitExpression, updateExpression.Parameters) as Expression<Func<T, T>>;
            return _repository.Update(getExpression, newUpdateExpression);
        }

        public int Update(IQueryable<T> query, Expression<Func<T, T>> updateExpression)
        {
            throw new NotImplementedException();
        }
    }
}
