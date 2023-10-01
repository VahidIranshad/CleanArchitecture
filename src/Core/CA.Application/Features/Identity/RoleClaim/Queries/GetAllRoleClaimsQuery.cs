using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Responses;
using MediatR;


namespace CA.Application.Features.Identity.RoleClaim.Queries
{
    public class GetAllRoleClaimsQuery : IRequest<List<RoleClaimResponse>>
    {
    }
    public class GetAllRoleClaimsQueryHandler : IRequestHandler<GetAllRoleClaimsQuery, List<RoleClaimResponse>>
    {

        private readonly IRoleClaimService _roleClaimService;

        public GetAllRoleClaimsQueryHandler(IRoleClaimService roleClaimService)
        {
            _roleClaimService = roleClaimService;
        }


        public async Task<List<RoleClaimResponse>> Handle(GetAllRoleClaimsQuery request, CancellationToken cancellationToken)
        {
            return await _roleClaimService.GetAllAsync();
        }
    }
}
