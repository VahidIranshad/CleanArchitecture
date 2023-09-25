using CA.Application.Contracts.Base;
using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;

namespace CA.Application.Contracts.Identity
{
    public interface IRoleClaimService : IService
    {
        Task<List<RoleClaimResponse>> GetAllAsync();

        Task<RoleClaimResponse> GetByIdAsync(int id);

        Task<List<RoleClaimResponse>> GetAllByRoleIdAsync(string roleId);

        Task<string> SaveAsync(RoleClaimRequest request);

        Task<string> DeleteAsync(int id);

        Task<bool> HasPermission(List<string> roleId, List<string> permissions);
        Task<bool> HasPermission(string userID, List<string> permissions);
    }
}