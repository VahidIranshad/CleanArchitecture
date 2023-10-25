using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Requests;
using MediatR;

namespace CA.Application.Features.Identity.RoleClaim.Commands
{
    public class InsertRoleClaimCommand : IRequest
    {
        public CreateRoleClaim Model { get; set; }
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
            var model = new RoleClaimRequest();
            model.Id = 0;
            var claim = CA.Domain.Constants.Permission.Permissions.AllPermision.GetAllPermision().First(p => p.Id == request.Model.Value);
            model.Group = claim.Group;
            model.Type = "Permision";
            model.Description = "CA";
            model.Value = request.Model.Value;
            model.RoleId = request.Model.RoleId;
            await _roleClaimService.SaveAsync(model);
            return Unit.Value;

        }

    }
}
