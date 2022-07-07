namespace RossBill.Bricks.Data
{
    using System.Threading.Tasks;

    public interface ICommandRepository<T> : IRepository<T>
        where T : class
    {
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);

        // Save changes is executed async
       Task<int> SaveChanges();
    }
}