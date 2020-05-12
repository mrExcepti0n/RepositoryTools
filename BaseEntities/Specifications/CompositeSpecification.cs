using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace BaseEntities.Specifications
{
    /// <summary>
    ///     ������������� ������������
    /// </summary>
    [DataContract]
    public abstract class CompositeSpecification : Specification
    {
        #region Fields And Properties

        private List<Specification> _components;

        /// <summary>
        ///     ������������� ������������
        /// </summary>
        protected ReadOnlyCollection<Specification> Components
        {
            get { return new ReadOnlyCollection<Specification>(_components); }
        }

        #endregion


        /// <summary>
        ///     �����������
        /// </summary>
        protected CompositeSpecification()
        {
            Initialize();
        }

        /// <summary>
        ///     �������� ������������� ������������
        /// </summary>
        /// <param name="childSpecification">������������� ������������</param>
        protected void AddChildComponent(Specification childSpecification)
        {
            if (!_components.Contains(childSpecification))
            {
                _components.Add(childSpecification);
            }
        }

        /// <summary>
        ///     ������� ��� ������������� ������������
        /// </summary>
        protected void ClearChildComponents()
        {
            _components.Clear();
        }

        /// <summary>
        ///     ������� ��������� ������������� ������������
        /// </summary>
        /// <param name="specification">������������� ������������</param>
        /// <returns>��� � � ����������</returns>
        protected bool RemoveChildComponent(Specification specification)
        {
            return _components.Remove(specification);
        }

        /// <summary>
        ///     ������� ��� ������ ����� �������������� ���������� ����������
        /// </summary>
        private void Initialize()
        {
            _components = new List<Specification>();
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext ctx)
        {
            Initialize();
        }

        /// <summary>
        ///     ���������� �������������� ��������� ������������
        /// </summary>
        /// <typeparam name="TCandidate">��� �������</typeparam>
        /// <param name="expressions">��������� ���� ������������� ������������</param>
        /// <returns>�������������� ������������</returns>
        protected abstract Expression<Func<TCandidate, bool>> Combine<TCandidate>(IEnumerable<Expression<Func<TCandidate, bool>>> expressions)
            where TCandidate : class;


        #region Overrides of Specification

        /// <inheritdoc />
        public sealed override Expression<Func<TCandidate, bool>> GetSatisfiedExpression<TCandidate>()
        {
            if (!_components.Any())
                return candidate => true;

            return Combine(_components.Select(component => component.GetSatisfiedExpression<TCandidate>()));
        }

        #endregion
    }
}