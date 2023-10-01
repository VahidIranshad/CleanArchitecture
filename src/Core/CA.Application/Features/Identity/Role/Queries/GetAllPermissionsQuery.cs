using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Responses;
using MediatR;

namespace CA.Application.Features.Identity.Role.Queries
{
    public class GetAllPermissionsQuery : IRequest<PermissionResponse>
    {
        public string roleId { get; set; }
    }
    public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, PermissionResponse>
    {

        private readonly IRoleService _roleService;

        public GetAllPermissionsQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }


        public async Task<PermissionResponse> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
        {
            return await _roleService.GetAllPermissionsAsync(request.roleId);
        }
    }
}
