using System.Collections.Generic;
using System.Threading.Tasks;

namespace RossBill.Bricks.Data
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IRepository<T>
        where T : class
    {
        /// <returns>The element with the given id.</returns>
        Task<T> GetById(int id);
            
        /// <returns>All the elements belonging to an entity</returns>
        IQueryable<T> GetAll();

        /// <returns>All the elements belonging to an entity, paged.</returns>
        IQueryable<T> GetAll(int pageNumber, int pageSize);

        /// <returns>All the elements belonging to an entity and responding to a where clause.</returns>
        IQueryable<T> GetAll(Expression<Func<T, bool>> whereClause);

        /// <returns>All the elements belonging to an entity and responding to a where clause, paged.</returns>
        IQueryable<T> GetAll(Expression<Func<T, bool>> whereClause, int pageNumber, int pageSize);

        /// <returns>projectionExpression - ie. Select</returns>
        IQueryable<X> GetAll<X>(Expression<Func<T, X>> projectionExpression);

        /// <returns>projectionExpression - ie. Select. Paged result.</returns>
        IQueryable<X> GetAll<X>(Expression<Func<T, X>> projectionExpression, int pageSize, int pageNumber);

        /// <returns>All the elements belonging to an entity and responding to a where clause, with a selection expression applied to them.</returns>
        IQueryable<X> GetAll<X>(Expression<Func<T, bool>> whereClause, Expression<Func<T, X>> projectionExpression);

        /// <returns>All the elements belonging to an entity and responding to a where clause, with a selection expression applied to them. Paged result.</returns>
        IQueryable<X> GetAll<X>(Expression<Func<T, bool>> whereClause, Expression<Func<T, X>> projectionExpression, int pageNumber, int pageSize);
    }
}