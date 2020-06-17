using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using BaseEntities.Specifications.Expressions;

namespace BaseEntities.Specifications
{
    [DataContract]
    public class CompositeOrSpecification : CompositeSpecification
    {

        [DataMember(IsRequired = true)]
        public Specification[] Members
        {
            get => Components.OfType<Specification>().ToArray();
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                ClearChildComponents();
                foreach (var specification in value)
                    AddChildComponent(specification);
            }
        }


        public CompositeOrSpecification(params Specification[] members)
        {
            Members = members;
        }

        public static Expression<Func<TCandidate, bool>> Or<TCandidate>(params Expression<Func<TCandidate, bool>>[] expressions)
        {
            if (expressions == null)
                return candidate => false;

            return Or(expressions as IEnumerable<Expression<Func<TCandidate, bool>>>);
        }

        public static Expression<Func<TCandidate, bool>> Or<TCandidate>(IEnumerable<Expression<Func<TCandidate, bool>>> expressions)
        {
            if (expressions == null)
                return candidate => false;

            var resultExpression = expressions.FirstOrDefault();
            if (resultExpression == null)
                return candidate => false;

            foreach (var another in expressions.Skip(1))
            {
                var parameterReplaceVisitor = new ParameterReplaceVisitor(resultExpression.Parameters[0], another.Parameters[0]);
                var resultAnother = parameterReplaceVisitor.Visit(another) as Expression<Func<TCandidate, bool>>;
                var resultInvoke = Expression.OrElse(resultExpression.Body, resultAnother.Body);
                resultExpression = Expression.Lambda<Func<TCandidate, bool>>(resultInvoke, resultExpression.Parameters[0]);
            }
            return resultExpression;
        }

        protected override Expression<Func<TCandidate, bool>> Combine<TCandidate>(IEnumerable<Expression<Func<TCandidate, bool>>> expressions)
        {
            return Or(expressions);
        }
    }
}