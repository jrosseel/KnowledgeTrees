namespace RossBill.Bricks.Cqrs.Command.Fluent
{
    using Validation;
    using Validation.Contracts;

    using FluentValidation;

    /// <summary>
    /// Adapter pattern that merges our validator, with the abstract validator of FluentValidation.
    /// 
    /// @createdby: Jente Rosseel
    /// @creationdate: 02/02/2015
    /// @lastmodified: 02/02/2015
    /// </summary>
    public abstract class FluentObjectValidator<T> : AbstractValidator<T>, IObjectValidator<T>
        where T : class
    {
        public new IValidationResult Validate(T objectToValidate)
        {
            // Create our validation result
            var myResult = new ValidationResult();
            // Get the validation of fluentvalidation
            var validationResult = base.Validate(objectToValidate);

            // If the result is valid, return our succeeded validation result
            if (validationResult.IsValid)
                return myResult;

            // If not, iterate all the validationErrors and add them to our validation result
            foreach (var errorItem in validationResult.Errors)
            {
                myResult.Errors.Add(
                    new ValidationError
                    {
                        // Fluentvalidation contains field validators, 
                        //  none field validators will be added in logic
                        IsFieldValidation = true,
                        FieldName = errorItem.PropertyName,
                        ErrorMessage = errorItem.ErrorMessage
                    }
                );
            }

            return myResult;
        }
    }
}