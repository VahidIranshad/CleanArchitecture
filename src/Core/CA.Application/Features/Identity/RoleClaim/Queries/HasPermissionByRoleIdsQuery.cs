using CA.Application.Contracts.Identity;
using MediatR;

namespace CA.Application.Features.Identity.RoleClaim.Queries
{
    public class HasPermissionByRoleIdsQuery : IRequest<bool>
    {
        public List<string> RoleIds { get; set; }
        public List<string> Permissions { get; set; }
    }
    public class HasPermissionByRoleIdsQueryHandler : IRequestHandler<HasPermissionByRoleIdsQuery, bool>
    {

        private readonly IRoleClaimService _roleClaimService;

        public HasPermissionByRoleIdsQueryHandler(IRoleClaimService roleClaimService)
        {
            _roleClaimService = roleClaimService;
        }


        public async Task<bool> Handle(HasPermissionByRoleIdsQuery request, CancellationToken cancellationToken)
        {
            return await _roleClaimService.HasPermission(request.RoleIds, request.Permissions);
        }
    }
}
