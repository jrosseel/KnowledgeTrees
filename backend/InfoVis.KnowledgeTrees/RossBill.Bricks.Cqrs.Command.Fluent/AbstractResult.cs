namespace RossBill.Bricks.Cqrs.Command.Fluent
{
    using Contracts;
    using Validation;
    using Validation.Contracts;
    
    /// <summary>
    /// Abstract command response class. 
    /// 
    /// @createdby: Jente Rosseel
    /// @creationdate: 02/02/2015
    /// @lastmodified: 02/02/2015
    /// </summary>
    public abstract class AbstractResult : IResult
    {
        protected AbstractResult()
        {
            ValidationResult = new ValidationResult();
        }

        public IValidationResult ValidationResult { get; set; }
    }
}
