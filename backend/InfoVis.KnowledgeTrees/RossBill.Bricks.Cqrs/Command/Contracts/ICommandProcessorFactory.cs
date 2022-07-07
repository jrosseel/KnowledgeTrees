namespace RossBill.Bricks.Cqrs.Command.Contracts
{
    /// <summary>
    /// Dispatcher for command execution
    /// </summary>
    public interface ICommandProcessorFactory
    {
        ICommandProcessor<TCommand, TResult> GetProcessor<TCommand, TResult>()
            where TCommand : class, ICommand
            where TResult : class, IResult; 
    }
}