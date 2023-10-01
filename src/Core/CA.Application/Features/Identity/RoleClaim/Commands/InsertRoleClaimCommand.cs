using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Requests;
using MediatR;

namespace CA.Application.Features.Identity.RoleClaim.Commands
{
    public class InsertRoleClaimCommand : IRequest
    {
        public RoleClaimRequest Model { get; set; }
    }
    public class InsertRoleClaimCommandHandler : IRequestHandler<InsertRoleClaimCommand>
    {

        private readonly IRoleClaimService _roleClaimService;

        public InsertRoleClaimCommandHandler(IRoleClaimService roleClaimService)
        {
            _roleClaimService = roleClaimService;
        }

        public async Task<Unit> Handle(InsertRoleClaimCommand request, CancellationToken cancellationToken)
        {
            request.Model.Id = 0;
            var claim = CA.Domain.Constants.Permission.Permissions.AllPermision.GetAllPermision().First(p => p.Id == request.Model.Value);
            request.Model.Group = claim.Group;
            request.Model.Type = "Permision";
            request.Model.Description = "CA";
            await _roleClaimService.SaveAsync(request.Model);
            return Unit.Value;

        }

    }
}
