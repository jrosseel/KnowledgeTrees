namespace RossBill.Bricks.Cqrs.Command.Validation.Contracts
{
    /// <summary>
    ///     Validates the bussiness commands
    /// 
    ///     @createdby: Jente Rosseel
    ///     @creationdate: 02/02/2015
    ///     @lastmodified: 02/02/2015
    /// </summary>
    /// <typeparam name="T">Bussiness command to validate</typeparam>
    public interface IObjectValidator<T> where T : class
    {
        /// <summary>
        ///     Validates the command
        /// </summary>
        /// <param name="objectToValidate">Command to validate</param>
        /// <returns>Result of the validation</returns>
        IValidationResult Validate(T objectToValidate);
    }
}
