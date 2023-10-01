using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Requests;
using MediatR;
namespace CA.Application.Features.Identity.Account.Commands
{
    public class UpdateProfileCommand : IRequest
    {
        public UpdateProfileRequest model { get; set; }
        public string userId { get; set; }
    }
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand>
    {

        private readonly IAccountService _accountService;

        public UpdateProfileCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<Unit> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            await _accountService.UpdateProfileAsync(request.model, request.userId);
            return Unit.Value;

        }

    }
}
