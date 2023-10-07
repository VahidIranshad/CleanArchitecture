using CA.Application.DTOs.Ent.Selection;
using FluentValidation;

namespace CA.Application.DTOs.Ent.Validators
{
    internal class SelectionUpdateValidator : AbstractValidator<SelectionUpdateDto>
    {
        public SelectionUpdateValidator()
        {

            RuleFor(p => p.Title)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");
            RuleFor(p => p.SelectionType)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
        }
    }
}
