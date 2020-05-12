using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace BaseEntities.Specifications
{
    public abstract class Specification : ISpecification
    {
        public AndSpecification And(Specification other)
        {
            return new AndSpecification(this, other);
        }

        /// <summary>
        ///     Выгрузить объекты не по этой спецификации
        /// </summary>
        /// <returns>Инвертирующая спецификация <see cref="InverseSpecification" /></returns>
        public InverseSpecification Not()
        {
            return new InverseSpecification(this);
        }

        /// <summary>
        ///     Выгрузить объекты по этой или другой спецификации
        /// </summary>
        /// <param name="other">Другая спецификация</param>
        /// <returns>Объединяющая спецификация <see cref="OrSpecification" /></returns>
        public OrSpecification Or(Specification other)
        {
            return new OrSpecification(this, other);
        }

        /// <inheritdoc />
        public virtual Expression<Func<TCandidate, bool>> GetSatisfiedExpression<TCandidate>() where TCandidate : class
        {
            throw new NotImplementedException();
        }


    }
}
