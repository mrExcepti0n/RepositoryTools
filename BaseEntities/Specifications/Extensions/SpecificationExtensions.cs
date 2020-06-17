using System.Linq;

namespace BaseEntities.Specifications.Extensions
{
    public static class SpecificationExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> queryable, ISpecification specification) where T : class
        {
            return specification == null
                ? queryable
                : queryable.Where(specification.GetSatisfiedExpression<T>());
        }
    }
}