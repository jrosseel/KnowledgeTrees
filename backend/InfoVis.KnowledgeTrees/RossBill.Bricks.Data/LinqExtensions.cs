namespace RossBill.Bricks.Data
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Contains methods that help working async with linq. 
    /// If linq to entities is dropped as database, change this because there is a strong dependency to it!
    /// </summary>
    public static class LinqExtensions
    {
        public static async Task<IEnumerable<T>> ExecuteQueryAsync<T>(this IQueryable<T> query)
        {
            return await query.ToListAsync();
        }
    }
}
