using System.Threading.Tasks;

namespace RossBill.Bricks.Data.Sql
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Repository using a SQL database stored over Entity Framework. Each defined entity has a repository which can query it.
    /// </summary>
    public class SqlRepository<T> : IRepository<T>
        where T : class
    {
        protected readonly DbContext _context;

        public SqlRepository(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("db context");

            _context = context;
        }
        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }

        public IQueryable<T> GetAll(int pageNumber, int pageSize)
        {
            return ApplySkipTake(GetAll(), pageNumber, pageSize);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> whereClause)
        {
            return GetAll().Where(whereClause).AsQueryable();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> whereClause, int pageNumber, int pageSize)
        {
            return ApplySkipTake(GetAll(whereClause), pageNumber, pageSize);
        }

        public IQueryable<X> GetAll<X>(Expression<Func<T, X>> projectionExpression)
        {
            return GetAll().Select(projectionExpression).AsQueryable();
        }

        public IQueryable<X> GetAll<X>(Expression<Func<T, X>> projectionExpression, int pageNumber, int pageSize)
        {
            return GetAll(pageNumber, pageSize)
                        .Select(projectionExpression).AsQueryable();
        }

        public IQueryable<X> GetAll<X>(Expression<Func<T, bool>> whereClause, Expression<Func<T, X>> projectionExpression)
        {
            return GetAll(whereClause).Select(projectionExpression).AsQueryable();
        }

        public IQueryable<X> GetAll<X>(Expression<Func<T, bool>> whereClause, Expression<Func<T, X>> projectionExpression, int pageNumber, int pageSize)
        {
            return GetAll(whereClause, pageNumber, pageSize) // Should be paged before executing the select for performance sake
                        .Select(projectionExpression).AsQueryable();
        }

        public async Task<T> GetById(int id)
        {
            var result = await _context.Set<T>().FindAsync(id);

            return result;
        }


        private static IQueryable<T> ApplySkipTake(IQueryable<T> query, int pageNumber, int pageSize)
        {
            return query.Skip(pageNumber * pageSize)
                        .Take(pageSize);
        }
    }
}