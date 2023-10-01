using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Requests;
using MediatR;

namespace CA.Application.Features.Identity.Account.Commands
{
    public class ChangePasswordCommand : IRequest
    {
        public ChangePasswordRequest model { get; set; }
        public string userId { get; set; }
    }
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
    {

        private readonly IAccountService _accountService;

        public ChangePasswordCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            await _accountService.ChangePasswordAsync(request.model, request.userId);
            return Unit.Value;

        }

    }
}
