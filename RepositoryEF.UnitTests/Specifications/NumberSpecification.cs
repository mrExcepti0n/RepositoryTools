using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BaseEntities.Helpers.Expressions;
using BaseEntities.Specifications;
using RepositoryEF.UnitTests.Model;

namespace RepositoryEF.UnitTests.Specifications
{
    public class NumberSpecification : Specification
    {
        public NumberSpecification(string number)
        {
            Number = number;
        }

        public string Number { get; }

        public override Expression<Func<TCandidate, bool>> GetSatisfiedExpression<TCandidate>()
        {
            Type candidateType = typeof(TCandidate);

            if (candidateType == typeof(Agreement))
            {
                Expression<Func<Agreement, bool>> sourceExpression = GetAgreementExpression();
                return ExpressionTransformer.Transform<Agreement, TCandidate>(sourceExpression);
            }


            return base.GetSatisfiedExpression<TCandidate>();
        }

        private Expression<Func<Agreement, bool>> GetAgreementExpression()
        {
            return agr => agr.Number == Number;
        }
    }
}
