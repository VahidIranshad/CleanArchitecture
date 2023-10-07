using FluentValidation;

namespace CA.Application.DTOs.Ent.TValue.Validators
{
    internal class TValueDtoValidator : AbstractValidator<TValueDto>
    {
        public TValueDtoValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");

        }
    }
}
