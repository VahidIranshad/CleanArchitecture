using CA.Application.Contracts.Generic;
using CA.Application.DTOs.Generic;
using FluentValidation;
using FluentValidation.Results;

namespace CA.Application.DTOs.Ent.TValue.Validators
{
    internal class DeleteTValueValidatior : AbstractValidator<TValueDto>, IBaseValidation
    {
        public DeleteTValueValidatior()
        {

        }
        public async Task<ValidationResult> ValidateAsync(IDto instance)
        {
            return base.Validate(instance as TValueDto);
        }

    }
}
