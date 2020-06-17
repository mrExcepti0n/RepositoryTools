using System;
using System.Linq.Expressions;

namespace BaseEntities.Specifications.Expressions
{
    public class ParameterReplaceVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _parameterExpression;
        private readonly ParameterExpression _targetParameterExpression;

       
        public ParameterReplaceVisitor(ParameterExpression parameterExpression) : this(parameterExpression, null)
        {
        }

        public ParameterReplaceVisitor(ParameterExpression parameterExpression, ParameterExpression targetParameterExpression)
        {
            _parameterExpression = parameterExpression ?? throw new ArgumentNullException(nameof(parameterExpression));
            _targetParameterExpression = targetParameterExpression;
        }


        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_targetParameterExpression != null && _targetParameterExpression != node)
                return node;

            return _parameterExpression;
        }
    }
}