using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Requests;
using MediatR;


namespace CA.Application.Features.Identity.User.Commands
{
    public class InsertUserRoleCommand : IRequest
    {
        public UpdateUserRolesRequest Request { get; set; }
    }
    public class InsertUserRoleCommandHandler : IRequestHandler<InsertUserRoleCommand>
    {

        private readonly IUserService _userService;

        public InsertUserRoleCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<Unit> Handle(InsertUserRoleCommand request, CancellationToken cancellationToken)
        {
            await _userService.InsertUserRolesAsync(request.Request);
            return Unit.Value;
        }
    }
}
