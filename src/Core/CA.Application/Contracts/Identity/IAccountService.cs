using CA.Application.Contracts.Base;
using CA.Application.DTOs.Identity.Requests;

namespace CA.Application.Contracts.Identity
{
    public interface IAccountService : IService
    {
        Task UpdateProfileAsync(UpdateProfileRequest model, string userId);

        Task ChangePasswordAsync(ChangePasswordRequest model, string userId);
    }
}
