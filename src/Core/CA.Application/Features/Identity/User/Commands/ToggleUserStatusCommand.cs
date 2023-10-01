using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Requests;
using MediatR;


namespace CA.Application.Features.Identity.User.Commands
{
    public class ToggleUserStatusCommand : IRequest
    {
        public ToggleUserStatusRequest Request { get; set; }
    }
    public class ToggleUserStatusCommandHandler : IRequestHandler<ToggleUserStatusCommand>
    {

        private readonly IUserService _userService;

        public ToggleUserStatusCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Unit> Handle(ToggleUserStatusCommand request, CancellationToken cancellationToken)
        {
            await _userService.ToggleUserStatusAsync(request.Request);
            return Unit.Value;

        }

    }
}
