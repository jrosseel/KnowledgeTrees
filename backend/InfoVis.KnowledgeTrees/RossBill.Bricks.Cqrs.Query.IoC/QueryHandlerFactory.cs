namespace RossBill.Bricks.Cqrs.Query.IoC
{
    using Contracts;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Factory that creates the query handlers using Unity IoC.
    /// 
    /// @createdby: Jente Rosseel
    /// @creationdate: 02/02/2015
    /// @lastmodified: 02/02/2015
    /// </summary>
    public class QueryHandlerFactory : IQueryHandlerFactory
    {
        private readonly IUnityContainer _container;

        public QueryHandlerFactory(IUnityContainer container)
        {
            _container = container;
        }

        public IQueryHandler<TQuery, TResponse> GetHandler<TQuery, TResponse>()
            where TQuery : class, IQuery
            where TResponse : class
        {
            return _container.Resolve<IQueryHandler<TQuery, TResponse>>();
        }
    }
}
