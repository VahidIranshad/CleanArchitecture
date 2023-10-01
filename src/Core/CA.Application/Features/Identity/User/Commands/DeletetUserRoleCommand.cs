using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Requests;
using MediatR;


namespace CA.Application.Features.Identity.User.Commands
{
    public class DeleteUserRoleCommand : IRequest
    {
        public UpdateUserRolesRequest Request { get; set; }
    }
    public class DeleteUserRoleCommandHandler : IRequestHandler<DeleteUserRoleCommand>
    {

        private readonly IUserService _userService;

        public DeleteUserRoleCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<Unit> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
        {
            await _userService.RemoveUserRoleAsync(request.Request);
            return Unit.Value;
        }
    }
}
