using CA.Application.Contracts.Identity;
using MediatR;

namespace CA.Application.Features.Identity.RoleClaim.Commands
{
    public class DeleteRoleClaimCommand : IRequest
    {
        public int Id { get; set; }
    }
    public class DeleteRoleClaimCommandHandler : IRequestHandler<DeleteRoleClaimCommand>
    {

        private readonly IRoleClaimService _roleClaimService;

        public DeleteRoleClaimCommandHandler(IRoleClaimService roleClaimService)
        {
            _roleClaimService = roleClaimService;
        }

        public async Task<Unit> Handle(DeleteRoleClaimCommand request, CancellationToken cancellationToken)
        {
            await _roleClaimService.DeleteAsync(request.Id);
            return Unit.Value;

        }

    }
}
