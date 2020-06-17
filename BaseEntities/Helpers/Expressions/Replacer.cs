using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BaseEntities.Helpers.Expressions
{
    internal class Replacer : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, Expression> _replacements;

        public Replacer(IEnumerable<ParameterExpression> what, IEnumerable<Expression> with)
        {
            _replacements = what.Zip(with, (param, expr) => new { param, expr }).ToDictionary(x => x.param, x => x.expr);
        }

        public Expression Replace(Expression body)
        {
            return Visit(body);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _replacements.TryGetValue(node,  out var replacement) ? replacement : base.VisitParameter(node);
        }
    }
}