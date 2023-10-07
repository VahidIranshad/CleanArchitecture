using CA.Application.Contracts.Generic;
using CA.Application.DTOs.Generic;
using FluentValidation;
using FluentValidation.Results;

namespace CA.Application.DTOs.Ent.TValue.Validators
{
    internal class UpdateTValueValidatior : AbstractValidator<TValueDto>, IBaseValidation
    {
        public UpdateTValueValidatior()
        {
            Include(new TValueDtoValidator());

        }

        public async Task<ValidationResult> ValidateAsync(IDto instance)
        {
            return base.Validate(instance as TValueDto);
        }
    }
}
