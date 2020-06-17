using System;
using System.Linq.Expressions;

namespace BaseEntities.Helpers.Expressions
{
    internal static class ExpressionReductor
    {
        public static T Apply<T, T1>(this Expression<Func<T1, T>> expression, T1 t1)
        {
            return expression.Compile()(t1);
        }

        public static Expression<T> Simplify<T>(this Expression<T> expression)
        {
            var invoker = new InvokerVisitor();
            return (Expression<T>)invoker.Visit(expression);
        }
    }
}