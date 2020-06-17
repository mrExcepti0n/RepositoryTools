using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace BaseEntities.Specifications
{
    public abstract class Specification : ISpecification
    {
        public AndSpecification And(Specification other)
        {
            return new AndSpecification(this, other);
        }

        public InverseSpecification Not()
        {
            return new InverseSpecification(this);
        }

        public OrSpecification Or(Specification other)
        {
            return new OrSpecification(this, other);
        }

        public virtual Expression<Func<TCandidate, bool>> GetSatisfiedExpression<TCandidate>() where TCandidate : class
        {
            throw new NotImplementedException();
        }
    }
}
