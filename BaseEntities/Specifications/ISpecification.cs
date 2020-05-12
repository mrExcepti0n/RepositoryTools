using System;
using System.Linq;
using System.Linq.Expressions;

namespace BaseEntities.Specifications
{
    /// <summary>
    ///     ��������� ������������ (���� ����� ������ ���), ���������� �� ����������.
    /// </summary>
    public interface ISpecification
    {
        /// <summary>
        ///     ���������� �������� ��� ������� �������� �� ���������
        /// </summary>
        /// <typeparam name="TCandidate">��� �������</typeparam>
        /// <returns>��������</returns>
        Expression<Func<TCandidate, bool>> GetSatisfiedExpression<TCandidate>() where TCandidate : class;
    }

    /// <summary>
    ///     ���������� ��� ���������� � ����������
    /// </summary>
    public static class ISpecificationExtension
    {
        /// <summary>
        ///     ���� <paramref name="specification">������������</paramref> ������, ������� ����� ������ �� ��������� ������� �
        ///     ���������, ����� ������� �������� ������
        /// </summary>
        /// <typeparam name="T">��� �������</typeparam>
        /// <param name="queryable">�������� ������</param>
        /// <param name="specification">��������� ������������</param>
        /// <returns>������ � ���������</returns>
        public static IQueryable<T> Where<T>(this IQueryable<T> queryable, ISpecification specification) where T : class
        {
            return specification == null
                ? queryable
                : queryable.Where(specification.GetSatisfiedExpression<T>());
        }
    }
}