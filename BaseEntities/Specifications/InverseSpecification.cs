using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace BaseEntities.Specifications
{
    [DataContract]
    public class InverseSpecification : CompositeSpecification
    {
        private Specification _toBeWrapped;

        [DataMember(IsRequired = true)]
        public Specification ToBeWrapped
        {
            get => _toBeWrapped;
            set
            {
                _toBeWrapped = value ?? throw new ArgumentNullException(nameof(value));
                ClearChildComponents();
                AddChildComponent(value);
            }
        }

       
        public InverseSpecification()
        {
        }

        public InverseSpecification(Specification toBeWrapped)
        {
            ToBeWrapped = toBeWrapped;
        }

        public static Expression<Func<TCandidate, bool>> Inverse<TCandidate>(Expression<Func<TCandidate, bool>> one)
        {
            var resultInvoke = Expression.Not(one.Body);
            var parameter = one.Parameters[0];
            return Expression.Lambda<Func<TCandidate, bool>>(resultInvoke, parameter);
        }

        protected override Expression<Func<TCandidate, bool>> Combine<TCandidate>(IEnumerable<Expression<Func<TCandidate, bool>>> expressions)
        {
            return Inverse(_toBeWrapped.GetSatisfiedExpression<TCandidate>());
        }
    }
}