namespace RossBill.Bricks.Cqrs.Command.Validation
{
    using Contracts;

    /// <summary>
    ///     Implementation of a validation result. Can be used as a DTO.
    /// 
    ///     @createdby: Jente Rosseel
    ///     @creationdate: 02/02/2015
    ///     @lastmodified: 02/02/2015
    /// </summary>
    public class ValidationError : IValidationError
    {
        public bool IsFieldValidation { get; set; }
        public string FieldName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
