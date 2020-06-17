using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace BaseEntities.Specifications
{

    [DataContract]
    public abstract class CompositeSpecification : Specification
    {
        private List<Specification> _components;


        protected ReadOnlyCollection<Specification> Components => new ReadOnlyCollection<Specification>(_components);


        protected CompositeSpecification()
        {
            Initialize();
        }

        protected void AddChildComponent(Specification childSpecification)
        {
            if (!_components.Contains(childSpecification))
            {
                _components.Add(childSpecification);
            }
        }

        protected void ClearChildComponents()
        {
            _components.Clear();
        }


        protected bool RemoveChildComponent(Specification specification)
        {
            return _components.Remove(specification);
        }


        private void Initialize()
        {
            _components = new List<Specification>();
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext ctx)
        {
            Initialize();
        }


        protected abstract Expression<Func<TCandidate, bool>> Combine<TCandidate>(IEnumerable<Expression<Func<TCandidate, bool>>> expressions)
            where TCandidate : class;


        public sealed override Expression<Func<TCandidate, bool>> GetSatisfiedExpression<TCandidate>()
        {
            if (!_components.Any())
                return candidate => true;

            return Combine(_components.Select(component => component.GetSatisfiedExpression<TCandidate>()));
        }
    }
}