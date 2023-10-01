using CA.Application.Contracts.Identity;
using MediatR;


namespace CA.Application.Features.Identity.User.Queries
{
    public class ConfirmUserEmailQuery : IRequest<string>
    {
        public string UserID { get; set; }
        public string Code { get; set; }
    }
    public class ConfirmUserEmailQueryQueryHandler : IRequestHandler<ConfirmUserEmailQuery, string>
    {

        private readonly IUserService _userService;

        public ConfirmUserEmailQueryQueryHandler(IUserService userService)
        {
            _userService = userService;
        }


        public async Task<string> Handle(ConfirmUserEmailQuery request, CancellationToken cancellationToken)
        {
            return await _userService.ConfirmEmailAsync(request.UserID, request.Code);
        }
    }
}
