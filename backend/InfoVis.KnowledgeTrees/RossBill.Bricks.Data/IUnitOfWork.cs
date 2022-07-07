namespace RossBill.Bricks.Data
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// A unit of work pattern that allows you to batch several CRUD operations into one call to the database. 
    /// Contains the command repositories for all writable entities of the project.
    /// Maps to one database.
    /// 
    /// Project specific implementation
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChanges();
    }
}