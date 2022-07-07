namespace RossBill.Bricks.Cqrs.Command.Contracts
{
    using Validation.Contracts;

    /// <summary>
    ///     Marker for the response of a command. Contains information about the execution (did it go well or not) 
    ///     + new values (like an ID upon create) that are the result of executing the command.
    /// 
    ///     @createdby: Jente Rosseel
    ///     @creationdate: 02/02/2015
    ///     @lastmodified: 02/02/2015
    /// </summary>
    public interface IResult
    {
        IValidationResult ValidationResult { get; set; }
    }
}
