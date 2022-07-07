namespace RossBill.Bricks.Cqrs.Command.Validation
{
    using Contracts;

    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     The result of a validation request. Is either valid or contains some errors.
    /// 
    ///     @createdby: Jente Rosseel
    ///     @creationdate: 02/02/2015
    ///     @lastmodified: 02/02/2015
    /// </summary>
    public class ValidationResult : IValidationResult
    {
        public ValidationResult()
        {
            Errors = new List<IValidationError>();
        }

        public bool IsValid
        {
            get { return !Errors.Any(); }
        }

        public IList<IValidationError> Errors { get; set; }

        public void AddError(string errorMessage)
        {
            AddError(errorMessage, null);
        }
        public void AddError(string errorMessage, string fieldName)
        {
            AddError(errorMessage, fieldName, fieldName != null);
        }
        private void AddError(string errorMessage, string fieldName, bool isField)
        {
            Errors.Add(
                new ValidationError
                {
                    ErrorMessage = errorMessage,
                    FieldName = fieldName,
                    IsFieldValidation = isField,
                }
            );
        }
    }
}