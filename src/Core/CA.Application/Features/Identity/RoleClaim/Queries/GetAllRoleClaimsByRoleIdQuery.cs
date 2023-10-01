using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Responses;
using MediatR;

namespace CA.Application.Features.Identity.RoleClaim.Queries
{
    public class GetAllRoleClaimsByRoleIdQuery : IRequest<List<RoleClaimResponse>>
    {
        public string roleId { get; set; }
    }
    public class GetAllByRoleIdQueryHandler : IRequestHandler<GetAllRoleClaimsByRoleIdQuery, List<RoleClaimResponse>>
    {

        private readonly IRoleClaimService _roleClaimService;

        public GetAllByRoleIdQueryHandler(IRoleClaimService roleClaimService)
        {
            _roleClaimService = roleClaimService;
        }


        public async Task<List<RoleClaimResponse>> Handle(GetAllRoleClaimsByRoleIdQuery request, CancellationToken cancellationToken)
        {
            return await _roleClaimService.GetAllByRoleIdAsync(request.roleId);
        }
    }
}
