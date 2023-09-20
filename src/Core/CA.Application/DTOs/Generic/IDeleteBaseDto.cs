using CA.Application.Contracts.Generic;

namespace CA.Application.DTOs.Generic
{
    public interface IDeleteBaseDto : IDto, IBaseDto
    {
        internal IBaseValidation BaseValidation { get; }
    }
}
