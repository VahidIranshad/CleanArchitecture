using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;
using MediatR;


namespace CA.Application.Features.Identity.Token.Queries
{
    public class LoginQuery : IRequest<TokenResponse>
    {
        public TokenRequest Request { get; set; }
    }
    public class LoginQueryHandler : IRequestHandler<LoginQuery, TokenResponse>
    {

        private readonly ITokenService _tokenService;

        public LoginQueryHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        public async Task<TokenResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            return await _tokenService.LoginAsync(request.Request);
        }
    }
}
