using System;
using System.Linq;
using System.Linq.Expressions;

namespace BaseEntities.Specifications
{
    /// <summary>
    ///     Интерфейс спецификации (есть такой шаблон ООП), основанный на предикатах.
    /// </summary>
    public interface ISpecification
    {
        /// <summary>
        ///     Возвращает предикат для выборки объектов из источника
        /// </summary>
        /// <typeparam name="TCandidate">Тип объекта</typeparam>
        /// <returns>Предикат</returns>
        Expression<Func<TCandidate, bool>> GetSatisfiedExpression<TCandidate>() where TCandidate : class;
    }

    /// <summary>
    ///     Расширение для интеграции с источником
    /// </summary>
    public static class ISpecificationExtension
    {
        /// <summary>
        ///     Если <paramref name="specification">спецификация</paramref> задана, выбрать новый запрос из исходного запроса к
        ///     источнику, иначе вернуть исходный запрос
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="queryable">Исходный запрос</param>
        /// <param name="specification">Возможная спецификация</param>
        /// <returns>Запрос к источнику</returns>
        public static IQueryable<T> Where<T>(this IQueryable<T> queryable, ISpecification specification) where T : class
        {
            return specification == null
                ? queryable
                : queryable.Where(specification.GetSatisfiedExpression<T>());
        }
    }
}