using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace BaseEntities.Specifications
{
    /// <summary>
    ///     Комбинирующая спецификация
    /// </summary>
    [DataContract]
    public abstract class CompositeSpecification : Specification
    {
        #region Fields And Properties

        private List<Specification> _components;

        /// <summary>
        ///     Комбинируемые спецификации
        /// </summary>
        protected ReadOnlyCollection<Specification> Components
        {
            get { return new ReadOnlyCollection<Specification>(_components); }
        }

        #endregion


        /// <summary>
        ///     Конструктор
        /// </summary>
        protected CompositeSpecification()
        {
            Initialize();
        }

        /// <summary>
        ///     Добавить комбинируемую спецификацию
        /// </summary>
        /// <param name="childSpecification">Комбинируемая спецификация</param>
        protected void AddChildComponent(Specification childSpecification)
        {
            if (!_components.Contains(childSpecification))
            {
                _components.Add(childSpecification);
            }
        }

        /// <summary>
        ///     Удалить все комбинируемые спецификации
        /// </summary>
        protected void ClearChildComponents()
        {
            _components.Clear();
        }

        /// <summary>
        ///     Удалить указанную комбинируемую спецификацию
        /// </summary>
        /// <param name="specification">Комбинируемая спецификация</param>
        /// <returns>Как и в коллекциях</returns>
        protected bool RemoveChildComponent(Specification specification)
        {
            return _components.Remove(specification);
        }

        /// <summary>
        ///     Костыль для работы через десериализацию клиентских контрактов
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
        ///     Реализация комбинирования выражений спецификаций
        /// </summary>
        /// <typeparam name="TCandidate">Тип объекта</typeparam>
        /// <param name="expressions">Выражения всех комбинируемых спецификаций</param>
        /// <returns>Результирующая спецификация</returns>
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