using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace BaseEntities.Specifications
{
    /// <summary>
    ///     Логика объединения ИЛИ для двух спецификаций
    /// </summary>
    [DataContract]
    public class OrSpecification : CompositeSpecification
    {
        #region Fields And Properties

        private Specification _one;
        private Specification _other;

        /// <summary>
        ///     Одна из спецификаций
        /// </summary>
        [DataMember(IsRequired = true)]
        public Specification One
        {
            get { return _one; }
            set
            {
#pragma warning disable IDE0016 // Использовать выражение "throw"
                if(value == null)
                    throw new ArgumentNullException(nameof(value));
#pragma warning restore IDE0016 // Использовать выражение "throw"

                if (Components.Contains(_one))
                    RemoveChildComponent(_one);
                _one = value;
                AddChildComponent(value);
            }
        }

        /// <summary>
        ///     Другая из спецификаций
        /// </summary>
        [DataMember(IsRequired = true)]
        public Specification Other
        {
            get { return _other; }
            set
            {
#pragma warning disable IDE0016 // Использовать выражение "throw"
                if(value == null)
                    throw new ArgumentNullException("value");
#pragma warning restore IDE0016 // Использовать выражение "throw"

                if (Components.Contains(_other))
                    RemoveChildComponent(_other);
                _other = value;
                AddChildComponent(value);
            }
        }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="one"></param>
        /// <param name="other"></param>
        public OrSpecification(Specification one, Specification other)
        {
            One = one;
            Other = other;
        }
        /// <summary>
        /// Предикат ИЛИ
        /// </summary>
        /// <typeparam name="TCandidate"></typeparam>
        /// <param name="one"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static Expression<Func<TCandidate, bool>> Or<TCandidate>(Expression<Func<TCandidate, bool>> one, Expression<Func<TCandidate, bool>> other)
            where TCandidate : class
        {
            return CompositeOrSpecification.Or(one, other);
        }


        #region Overrides of CompositeSpecification

        /// <inheritdoc />
        protected override Expression<Func<TCandidate, bool>> Combine<TCandidate>(IEnumerable<Expression<Func<TCandidate, bool>>> expressions)
        {
            return CompositeOrSpecification.Or(expressions);
        }

        #endregion
    }
}