using BaseEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RepositoryAbstraction
{
    public class WriteTrackingEntityRepository<T, TKey> : IWriteRepository<T, TKey> where T : class, ITable<TKey>,  new() where TKey : struct
    {
        public WriteTrackingEntityRepository(WriteBaseRepository<T, TKey> repository, IIdentityProvider identityProvider)
        {
            _repository = repository;
            _isPhysicalDelete = false;
            _identityProvider = identityProvider;
        }
        
        private readonly IIdentityProvider _identityProvider;
        private readonly WriteBaseRepository<T, TKey> _repository;
        private readonly bool _isPhysicalDelete;

        private void ApplyUpdateDate(IEnumerable<IChangeDate> entityCollection)
        {
            var dateTimeNow = DateTime.Now;
            foreach (var element in entityCollection)
            {
                element.ChangeDate = dateTimeNow;
            }
        }

        private void ApplyWorkerChangers(IEnumerable<IChangedBy> entityCollection)
        {
            foreach (IChangedBy element in entityCollection)
            {
                element.ChangedBy = _identityProvider.User;
            }
        }

        private int SoftDelete(IEnumerable<T> entities)
        {
            var entityCollection = entities.ToList();

            ApplyUpdateDate(entityCollection);
            ApplyWorkerChangers(entityCollection);
            foreach (var element in entityCollection)
            {
                element.IsDeleted = true;
            }

            return entityCollection.Count;
        }


        public void Add(params T[] entities)
        {
            Add(entities as IEnumerable<T>);
        }

        public void Add(IEnumerable<T> entities)
        {
            var entityCollection = entities.ToList();

            ApplyUpdateDate(entityCollection);
            ApplyWorkerChangers(entityCollection);
            _repository.Add(entityCollection);
        }

        public void BulkInsert(IEnumerable<T> entities, int? batchSize = 50000, int? commandTimeout = 300, BulkCopyOptions? bulkCopyOptions = BulkCopyOptions.Default)
        {
            var entityCollection = entities.ToList();
            ApplyUpdateDate(entityCollection);
            ApplyWorkerChangers(entityCollection);
            _repository.BulkInsert(entityCollection, batchSize, commandTimeout, bulkCopyOptions);
        }

        public int Delete(T entity)
        {            
            return Delete(new List<T> { entity });
        }

        public int Delete(IEnumerable<T> entities)
        {
            return _isPhysicalDelete ? _repository.Delete(entities) : SoftDelete(entities);
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
            return Delete(entity => idCollection.Contains(entity.Id));
        }

        //TODO move save changes to unit of work, and rewrite apply entity fields logic to avoid overfilling
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

    }
}
