using CA.Application.Contracts.Base;
using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;

namespace CA.Application.Contracts.Identity
{
    public interface IUserService : IService
    {
        Task<List<UserResponse>> GetAllAsync();

        Task<int> GetCountAsync();

        Task<UserResponse> GetAsync(string userId);

        Task RegisterAsync(RegisterRequest request, string origin);

        Task ToggleUserStatusAsync(ToggleUserStatusRequest request);

        Task<List<UserRoleModel>> GetRolesAsync(string id);
        
        Task InsertUserRolesAsync(UpdateUserRolesRequest request);
        Task RemoveUserRoleAsync(UpdateUserRolesRequest request);


        Task<string> ConfirmEmailAsync(string userId, string code);

        Task ForgotPasswordAsync(ForgotPasswordRequest request, string origin);

        Task ResetPasswordAsync(ResetPasswordRequest request);

        Task<string> ExportToExcelAsync(string searchString = "");
        Task<(List<UserResponse>, int)> Get(string Filter, string Order, int? PageNumber, int? PageSize, bool? disableTracking = true);
    }
}