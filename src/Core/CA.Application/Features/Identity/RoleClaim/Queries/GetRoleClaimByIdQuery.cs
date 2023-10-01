using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Responses;
using MediatR;

namespace CA.Application.Features.Identity.RoleClaim.Queries
{
    public class GetRoleClaimByIdQuery : IRequest<RoleClaimResponse>
    {
        public int ID { get; set; }
    }
    public class GetRoleClaimByIdQueryHandler : IRequestHandler<GetRoleClaimByIdQuery, RoleClaimResponse>
    {

        private readonly IRoleClaimService _roleClaimService;

        public GetRoleClaimByIdQueryHandler(IRoleClaimService roleClaimService)
        {
            _roleClaimService = roleClaimService;
        }


        public async Task<RoleClaimResponse> Handle(GetRoleClaimByIdQuery request, CancellationToken cancellationToken)
        {
            return await _roleClaimService.GetByIdAsync(request.ID);
        }
    }
}
