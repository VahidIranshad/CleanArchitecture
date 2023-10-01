using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Requests;
using MediatR;


namespace CA.Application.Features.Identity.User.Commands
{
    public class ForgotPasswordCommand : IRequest
    {
        public ForgotPasswordRequest Request { get; set; }
        public string Origin { get; set; }
    }
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {

        private readonly IUserService _userService;

        public ForgotPasswordCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            await _userService.ForgotPasswordAsync(request.Request, request.Origin);
            return Unit.Value;
        }
    }
}
