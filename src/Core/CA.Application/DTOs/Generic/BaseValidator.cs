using FluentValidation;

namespace CA.Application.DTOs.Generic
{
    public abstract class BaseValidator<T> : AbstractValidator<T>
       where T : IDto
    {
    }
}
