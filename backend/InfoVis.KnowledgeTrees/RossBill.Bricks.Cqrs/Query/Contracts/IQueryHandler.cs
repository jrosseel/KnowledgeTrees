using System.Threading.Tasks;

namespace RossBill.Bricks.Cqrs.Query.Contracts
{
    /// <summary>
    /// Base interface for query handlers
    /// 
    /// @createdby: Jente Rosseel
    /// @creationdate: 02/02/2015
    /// @lastmodified: 02/02/2015
    /// </summary>
    /// <typeparam name="TQuery">Query type</typeparam>
    /// <typeparam name="TInformation">The information that result from the query.</typeparam>
    public interface IQueryHandler<in TQuery, TInformation>
        where TQuery : IQuery
        where TInformation : class
    {
        /// <summary>
        /// Retrieve a query result from a query
        /// </summary>
        /// <param name="query">Query</param>
        /// <returns>Retrieve Query Result</returns>
        Task<TInformation> Retrieve(TQuery query);
    }
}
