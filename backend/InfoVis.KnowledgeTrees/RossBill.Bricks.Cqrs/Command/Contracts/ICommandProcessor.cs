namespace RossBill.Bricks.Cqrs.Command.Contracts
{
    using System.Threading.Tasks;

    /// <summary>
    ///     Executes a command
    /// 
    ///     @createdby: Jente Rosseel
    ///     @creationdate: 02/02/2015
    ///     @lastmodified: 02/02/2015
    /// </summary>
    /// <typeparam name="T">The type of Command</typeparam>
    /// <typeparam name="R">The type of Command Response</typeparam>
    public interface ICommandProcessor<T, R>
        where T : class, ICommand
        where R : class, IResult
    {
        Task<R> Execute(T command);
    }
}
