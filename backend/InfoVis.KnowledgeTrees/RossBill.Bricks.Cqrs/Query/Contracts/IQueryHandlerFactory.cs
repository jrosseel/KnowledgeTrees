namespace RossBill.Bricks.Cqrs.Query.Contracts
{
    /// <summary>
    /// Dispatcher for the right query handler
    /// 
    /// @createdby: Jente Rosseel
    /// @creationdate: 02/02/2015
    /// @lastmodified: 02/02/2015
    /// </summary>
    public interface IQueryHandlerFactory
    {
        IQueryHandler<TQuery, TResponse> GetHandler<TQuery, TResponse>()
            where TQuery : class, IQuery
            where TResponse : class;
    }
}
