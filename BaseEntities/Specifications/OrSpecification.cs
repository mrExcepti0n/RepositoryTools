using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace BaseEntities.Specifications
{
    [DataContract]
    public class OrSpecification : CompositeSpecification
    {
        private Specification _one;
        private Specification _other;


        [DataMember(IsRequired = true)]
        public Specification One
        {
            get => _one;
            set
            {
                if(value == null)
                    throw new ArgumentNullException(nameof(value));

                if (Components.Contains(_one))
                    RemoveChildComponent(_one);
                _one = value;
                AddChildComponent(value);
            }
        }

        [DataMember(IsRequired = true)]
        public Specification Other
        {
            get => _other;
            set
            {
                if(value == null)
                    throw new ArgumentNullException(nameof(value));

                if (Components.Contains(_other))
                    RemoveChildComponent(_other);
                _other = value;
                AddChildComponent(value);
            }
        }

        public OrSpecification(Specification one, Specification other)
        {
            One = one;
            Other = other;
        }

        public static Expression<Func<TCandidate, bool>> Or<TCandidate>(Expression<Func<TCandidate, bool>> one, Expression<Func<TCandidate, bool>> other)
            where TCandidate : class
        {
            return CompositeOrSpecification.Or(one, other);
        }


        protected override Expression<Func<TCandidate, bool>> Combine<TCandidate>(IEnumerable<Expression<Func<TCandidate, bool>>> expressions)
        {
            return CompositeOrSpecification.Or(expressions);
        }
    }
}