namespace RossBill.Bricks.Cqrs.Command.Validation.Contracts
{
    using System.Collections.Generic;

    /// <summary>
    ///     Contains the result of a validation.
    ///     
    ///     @createdby: Jente Rosseel
    ///     @creationdate: 02/02/2015
    ///     @lastmodified: 02/02/2015
    /// </summary>
    public interface IValidationResult
    {
        bool IsValid { get; }

        IList<IValidationError> Errors { get; set; }

        void AddError(string errorMessage);
        void AddError(string errorMessage, string fieldName);
    }
}