using System;
using System.Linq.Expressions;
using System.Reflection;

namespace BaseEntities.Specifications.Expressions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        /// Преобразовать выражение
        /// </summary>
        /// <typeparam name="TCandidate"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="sourceExpression"></param>
        /// <returns></returns>
        public static Expression<Func<TCandidate, bool>> ConvertQueryExpression<TCandidate, TSource>(Expression<Func<TSource, bool>> sourceExpression)
        {
            var correctParameter = Expression.Parameter(
                typeof(TCandidate),
                $"Требуемый параметр типа из {typeof(ExpressionHelper).FullName}, {MethodBase.GetCurrentMethod().Name}");
            var correctExpression = new ParameterReplaceVisitor(correctParameter, sourceExpression.Parameters[0]).Visit(sourceExpression.Body);
            return Expression.Lambda<Func<TCandidate, bool>>(correctExpression, correctParameter);
        }
    }
}