using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Requests;
using MediatR;

namespace CA.Application.Features.Identity.User.Commands
{
    public class ResetPasswordCommand : IRequest
    {
        public ResetPasswordRequest Request { get; set; }
    }
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {

        private readonly IUserService _userService;

        public ResetPasswordCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            await _userService.ResetPasswordAsync(request.Request);
            return Unit.Value;
        }
    }
}
