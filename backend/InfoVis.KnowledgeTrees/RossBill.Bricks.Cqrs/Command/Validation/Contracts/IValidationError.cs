namespace RossBill.Bricks.Cqrs.Command.Validation.Contracts
{
    /// <summary>
    ///     A validation error for a bussiness field. Gets thrown back up to user level where it can be handled properly.
    /// 
    ///     @createdby: Jente Rosseel
    ///     @creationdate: 02/02/2015
    ///     @lastmodified: 02/02/2015
    /// </summary>
    public interface IValidationError
    {
        bool IsFieldValidation { get; set; }
        string FieldName { get; set; }

        string ErrorMessage { get; set; }
    }
}
