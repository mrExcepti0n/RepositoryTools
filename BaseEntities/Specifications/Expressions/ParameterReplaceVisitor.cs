using System;
using System.Linq.Expressions;

namespace BaseEntities.Specifications.Expressions
{
    /// <summary>
    ///     Посетитель выражения, подменяющий все либо указанные подменяемые параметры на указанный
    /// </summary>
    public class ParameterReplaceVisitor : ExpressionVisitor
    {
        #region Fields And Properties

        private readonly ParameterExpression _parameterExpression;
        private readonly ParameterExpression _targetParameterExpression;

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="parameterExpression"></param>
        public ParameterReplaceVisitor(ParameterExpression parameterExpression) : this(parameterExpression, null)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="parameterExpression">Подменяющий параметр (обязательный)</param>
        /// <param name="targetParameterExpression">Необязательный подменяемый параметр</param>
        public ParameterReplaceVisitor(ParameterExpression parameterExpression, ParameterExpression targetParameterExpression)
        {
            _parameterExpression = parameterExpression ?? throw new ArgumentNullException(nameof(parameterExpression));
            _targetParameterExpression = targetParameterExpression;
        }


        #region Overrides of ExpressionVisitor

        /// <summary>
        ///     Замена параметров на указанный/>.
        /// </summary>
        /// <returns>
        ///     Измененное выражение в случае изменения самого выражения или любого его подвыражения; в противном случае
        ///     возвращается исходное выражение.
        /// </returns>
        /// <param name="node">Выражение, которое необходимо просмотреть.</param>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_targetParameterExpression != null && _targetParameterExpression != node)
                return node;

            return _parameterExpression;
        }

        #endregion
    }
}