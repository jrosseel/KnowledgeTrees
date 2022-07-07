namespace RossBill.Bricks.Data
{
    using System;

    /// <summary>
    /// A read-only implementation of the repository pattern. Provides lazy access to the read-repositories of all readable entities of the project.
    /// Maps to one database.
    ///
    /// Project specific implementation
    /// </summary>
    public interface IQueryHolder : IDisposable { }
}