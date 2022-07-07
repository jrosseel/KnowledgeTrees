using System.Threading.Tasks;

namespace RossBill.Bricks.Data.Sql
{
    using System;
    using System.Linq;
    using System.Data.Entity;

    /// <summary>
    /// Reposit
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SqlCommandRepository<T> : SqlRepository<T>, ICommandRepository<T>
        where T : class
    {
        public SqlCommandRepository(DbContext context)
            : base(context)
        {
        }

        public void Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var entry = _context.Entry(entity);
            entry.State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public Task<int> SaveChanges()
        {
            return _context.SaveChangesAsync();
        }
    }
}