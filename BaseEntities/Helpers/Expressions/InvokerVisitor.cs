using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BaseEntities.Helpers.Expressions
{
    public class InvokerVisitor : ExpressionVisitor
    {
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(ExpressionReductor) && node.Method.Name == "Apply")
            {
                var lambda = GetLambda(node.Arguments[0]);
                return Replace(lambda, node.Arguments.Skip(1));
            }
            return base.VisitMethodCall(node);
        }

        private Expression Replace(LambdaExpression lambda, IEnumerable<Expression> arguments)
        {
            var replacer = new Replacer(lambda.Parameters, arguments);
            return replacer.Replace(lambda.Body);
        }


        private LambdaExpression GetLambda(Expression expression)
        {
            var finder = new FieldLambdaFinder();
            return (LambdaExpression)finder.Find(expression);
        }
    }
}