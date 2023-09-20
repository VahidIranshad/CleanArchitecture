using FluentValidation.Results;

namespace CA.Application.Exceptions
{
    public class CustomValidationException : ApplicationException
    {
        public List<string> Errors { get; set; } = new List<string>();

        public CustomValidationException(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Errors.Add(error.ErrorMessage);
            }
        }
    }
}
