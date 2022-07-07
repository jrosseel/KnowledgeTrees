namespace RossBill.Bricks.Cqrs.Query.IoC
{
    using Command.Contracts;

    using Microsoft.Practices.Unity;

    public class CommandProcessorFactory : ICommandProcessorFactory
    {
        private readonly IUnityContainer _container;

        public CommandProcessorFactory(IUnityContainer container)
        {
            _container = container;
        }

        public ICommandProcessor<TCommand, TResult> GetProcessor<TCommand, TResult>()
            where TCommand : class, ICommand
            where TResult : class, IResult
        {
            // Late resolving of dependencies
            return _container.Resolve<ICommandProcessor<TCommand, TResult>>();
        }
    }
}