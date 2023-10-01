using CA.Application.Contracts.Identity;
using MediatR;


namespace CA.Application.Features.Identity.RoleClaim.Queries
{
    public class HasPermissionByUserIDQuery : IRequest<bool>
    {
        public string UserID { get; set; }
        public List<string> Permissions { get; set; }
    }
    public class HasPermissionByUserIDQueryHandler : IRequestHandler<HasPermissionByUserIDQuery, bool>
    {

        private readonly IRoleClaimService _roleClaimService;

        public HasPermissionByUserIDQueryHandler(IRoleClaimService roleClaimService)
        {
            _roleClaimService = roleClaimService;
        }


        public async Task<bool> Handle(HasPermissionByUserIDQuery request, CancellationToken cancellationToken)
        {
            return await _roleClaimService.HasPermission(request.UserID, request.Permissions);
        }
    }
}
