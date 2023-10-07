using CA.Application.Contracts.Generic;
using CA.Application.DTOs.Generic;
using FluentValidation;
using FluentValidation.Results;

namespace CA.Application.DTOs.Ent.TValue.Validators
{
    internal class CreateTValueValidatior : AbstractValidator<TValueDto>, IBaseValidation
    {
        public CreateTValueValidatior()
        {
            Include(new TValueDtoValidator());

        }

        public async Task<ValidationResult> ValidateAsync(IDto instance)
        {
            return base.Validate(instance as TValueDto);
        }
    }
}
