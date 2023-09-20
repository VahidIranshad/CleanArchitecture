using CA.Application.Contracts.Base;

namespace CA.Application.Contracts.Identity
{
    public interface ICurrentUserService : IService
    {
        string UserId { get; }
    }
}
