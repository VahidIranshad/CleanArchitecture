using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Requests;
using MediatR;

namespace CA.Application.Features.Identity.User.Commands
{
    public class RegisterUserCommand : IRequest
    {
        public RegisterRequest Request { get; set; }
        public string Origin { get; set; }
    }
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
    {

        private readonly IUserService _userService;

        public RegisterUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            await _userService.RegisterAsync(request.Request, request.Origin);
            return Unit.Value;

        }

    }
}
