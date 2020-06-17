using System;
using System.Linq.Expressions;

namespace BaseEntities.Specifications
{

    public interface ISpecification
    {
        Expression<Func<TCandidate, bool>> GetSatisfiedExpression<TCandidate>() where TCandidate : class;
    }
}