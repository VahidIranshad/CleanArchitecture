using CA.Application.Contracts.Generic;

namespace CA.Application.DTOs.Generic
{
    public interface IUpdateBaseDto : IDto, IBaseDto
    {
        internal IBaseValidation BaseValidation { get; }
    }
}
