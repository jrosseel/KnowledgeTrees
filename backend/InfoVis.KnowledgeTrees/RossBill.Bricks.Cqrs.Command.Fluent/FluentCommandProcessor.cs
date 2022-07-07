namespace RossBill.Bricks.Cqrs.Command.Fluent
{
    using System.Threading.Tasks;

    using Contracts;
    using Validation.Contracts;
    
    /// <summary>
    /// Abstract implementation of a processor, which will validate the command before executing it.
    /// 
    /// @createdby: Jente Rosseel
    /// @creationdate: 02/02/2015
    /// @lastmodified: 02/02/2015
    /// </summary>
    /// <typeparam name="T">Command</typeparam>
    /// <typeparam name="R">Response</typeparam>
    public abstract class FluentCommandProcessor<T, R> : ICommandProcessor<T, R>
        where T : class, ICommand
        where R : class, IResult
    {
        protected IObjectValidator<T> _validator;

        public FluentCommandProcessor(IObjectValidator<T> validator)
        {
            _validator = validator;
        }

        /// <summary>
        ///     Function that instantiates of the appropriate ICommandResponse
        /// </summary>
        /// <returns>The appropriate ICommandResponse</returns>
        protected abstract R InstantiateResponse();

        /// <summary>
        ///     Executes the logic of the command, 
        ///     
        ///     Do not forget to perform validation against the database here or in an appropriate decorator
        /// </summary>
        /// <param name="command">The command the execute</param>
        /// <returns>The command response</returns>
        protected abstract Task<R> ExecuteCommand(T command);

        public async Task<R> Execute(T command)
        {
            R response = InstantiateResponse();
            response.ValidationResult = _validator.Validate(command);

            if (response.ValidationResult.IsValid)
            {
                response = await ExecuteCommand(command);
            }

            return response;
        }
    }
}
