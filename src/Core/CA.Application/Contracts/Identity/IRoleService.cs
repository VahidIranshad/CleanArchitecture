using CA.Application.Contracts.Base;
using CA.Application.DTOs.Identity.Responses;

namespace CA.Application.Contracts.Identity
{
    public interface IRoleService : IService
    {
        Task<List<RoleResponse>> GetAllAsync();

        Task<int> GetCountAsync();

        Task<RoleResponse> GetByIdAsync(string id);

        Task<PermissionResponse> GetAllPermissionsAsync(string roleId);

        Task<(List<RoleResponse>, int)> Get(string Filter, string Order, int? PageNumber, int? PageSize, bool? disableTracking = true);
    }
}