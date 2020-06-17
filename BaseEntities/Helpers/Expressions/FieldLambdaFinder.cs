using System.Linq.Expressions;
using System.Reflection;

namespace BaseEntities.Helpers.Expressions
{
    internal class FieldLambdaFinder : ExpressionVisitor
    {
        protected override Expression VisitMember(MemberExpression node)
        {
            var constantExpression = (ConstantExpression)node.Expression;
            var info = (FieldInfo)node.Member;
            var fieldValue = (Expression)info.GetValue(constantExpression.Value);
            return fieldValue;
        }

        public Expression Find(Expression expression)
        {
            return Visit(expression);
        }
    }
}