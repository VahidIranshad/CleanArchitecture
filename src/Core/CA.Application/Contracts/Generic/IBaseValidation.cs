using CA.Application.DTOs.Generic;
using FluentValidation.Results;

namespace CA.Application.Contracts.Generic
{
    public interface IBaseValidation
    {
        Task<ValidationResult> ValidateAsync(IDto instance);
    }
}
