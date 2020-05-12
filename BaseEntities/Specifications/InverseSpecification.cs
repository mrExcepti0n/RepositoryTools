using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace BaseEntities.Specifications
{
    /// <summary>
    ///     ������ ������: ���������
    /// </summary>
    [DataContract]
    public class InverseSpecification : CompositeSpecification
    {
        #region Fields And Properties

        private Specification _toBeWrapped;

        /// <summary>
        ///     ��� ����������
        /// </summary>
        [DataMember(IsRequired = true)]
        public Specification ToBeWrapped
        {
            get => _toBeWrapped;
            set
            {
                _toBeWrapped = value ?? throw new ArgumentNullException(nameof(value));
                ClearChildComponents();
                AddChildComponent(value);
            }
        }

        #endregion

        /// <inheritdoc />
        [Obsolete("�� ������������, ������ ��� WCF")]
        public InverseSpecification()
        {
        }

        /// <inheritdoc />
        public InverseSpecification(Specification toBeWrapped)
        {
            ToBeWrapped = toBeWrapped;
        }
        /// <summary>
        /// ���������
        /// </summary>
        /// <typeparam name="TCandidate"></typeparam>
        /// <param name="one"></param>
        /// <returns></returns>
        public static Expression<Func<TCandidate, bool>> Inverse<TCandidate>(Expression<Func<TCandidate, bool>> one)
        {
            var resultInvoke = Expression.Not(one.Body);
            var parameter = one.Parameters[0];
            return Expression.Lambda<Func<TCandidate, bool>>(resultInvoke, parameter);
        }


        #region Overrides of CompositeSpecification

        /// <inheritdoc />
        protected override Expression<Func<TCandidate, bool>> Combine<TCandidate>(IEnumerable<Expression<Func<TCandidate, bool>>> expressions)
        {
            return Inverse(_toBeWrapped.GetSatisfiedExpression<TCandidate>());
        }

        #endregion
    }
}