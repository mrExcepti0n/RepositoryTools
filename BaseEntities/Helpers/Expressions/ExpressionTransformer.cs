using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BaseEntities.Helpers.Expressions
{
    public static class ExpressionTransformer
    {
        private class Visitor : ExpressionVisitor
        {
            private ParameterExpression _parameter;


            private readonly ParameterExpression _parameterExpression;
            private readonly ParameterExpression _targetParameterExpression;

            public Visitor(ParameterExpression parameter)
            {
                _parameter = parameter;
            }

            public Visitor(ParameterExpression parameterExpression, ParameterExpression targetParameterExpression)
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

        public static Expression<Func<TTo, bool>> Transform<TFrom, TTo>(Expression<Func<TFrom, bool>> expression)
        {


            var correctParameter = Expression.Parameter(
                typeof(TTo),$"Требуемый параметр типа из {typeof(ExpressionTransformer).FullName}, {MethodBase.GetCurrentMethod().Name}");
            var correctExpression = new Visitor(correctParameter, expression.Parameters[0]).Visit(expression.Body);
            return Expression.Lambda<Func<TTo, bool>>(correctExpression, correctParameter);
        }

        public static Expression<Func<TTo, bool>> Transform<TFrom, TTo>(Expression<Func<TFrom, bool>> expression, ParameterExpression parameter)
        {
            var fromParameter = expression.Parameters.First();
            var body = new Visitor(parameter, fromParameter).Visit(expression.Body);
            return Expression.Lambda<Func<TTo, bool>>(body, parameter);
        }
    }
}
