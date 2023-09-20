using CA.Application.Contracts.Generic;

namespace CA.Application.DTOs.Generic
{
    public interface ICreateBaseDto : IDto
    {
        internal IBaseValidation BaseValidation { get; }
    }
}
