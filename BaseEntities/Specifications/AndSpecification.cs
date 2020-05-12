using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace BaseEntities.Specifications
{
    /// <summary>
    ///     ������ ����������� � ��� ���� ������������
    /// </summary>
    [DataContract]
    public class AndSpecification : CompositeSpecification
    {
        #region Fields And Properties

        private Specification _one;
        private Specification _other;

        /// <summary>
        ///     ���� �� ������������
        /// </summary>
        [DataMember(IsRequired = true)]
        public Specification One
        {
            get => _one;
            set
            {
                if (Components.Contains(_one))
                    RemoveChildComponent(_one);
                _one = value ?? throw new ArgumentNullException(nameof(value));
                AddChildComponent(value);
            }
        }

        /// <summary>
        ///     ������ �� ������������
        /// </summary>
        [DataMember(IsRequired = true)]
        public Specification Other
        {
            get { return _other; }
            set
            {
                if (value == null)
#pragma warning disable IDE0016 // ������������ ��������� "throw"
                    throw new ArgumentNullException("value");
#pragma warning restore IDE0016 // ������������ ��������� "throw"

                if (Components.Contains(_other))
                    RemoveChildComponent(_other);
                _other = value;
                AddChildComponent(value);
            }
        }

        #endregion

        /// <summary>
        /// �����������
        /// </summary>
        [Obsolete("�� ������������, ������ ��� WCF")]
        public AndSpecification()
        {
        }
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="one"></param>
        /// <param name="other"></param>
        public AndSpecification(Specification one, Specification other)
        {
            One = one;
            Other = other;
        }
        /// <summary>
        /// �������� �����������
        /// </summary>
        /// <typeparam name="TCandidate"></typeparam>
        /// <param name="one"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static Expression<Func<TCandidate, bool>> And<TCandidate>(Expression<Func<TCandidate, bool>> one, Expression<Func<TCandidate, bool>> other)
            where TCandidate : class
        {
            return CompositeAndSpecification.And(one, other);
        }


        #region Overrides of CompositeSpecification
        /// <summary>
        /// �������� ��������������
        /// </summary>
        /// <typeparam name="TCandidate"></typeparam>
        /// <param name="expressions"></param>
        /// <returns></returns>
        protected override Expression<Func<TCandidate, bool>> Combine<TCandidate>(IEnumerable<Expression<Func<TCandidate, bool>>> expressions)
        {
            return CompositeAndSpecification.And(expressions);
        }

        #endregion
    }
}