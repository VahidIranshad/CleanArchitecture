using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;
using MediatR;

namespace CA.Application.Features.Identity.Token.Queries
{
    public class GetRefreshTokenQuery : IRequest<TokenResponse>
    {
        public RefreshTokenRequest Request { get; set; }
    }
    public class GetRefreshTokenQueryHandler : IRequestHandler<GetRefreshTokenQuery, TokenResponse>
    {

        private readonly ITokenService _tokenService;

        public GetRefreshTokenQueryHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        public async Task<TokenResponse> Handle(GetRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            return await _tokenService.GetRefreshTokenAsync(request.Request);
        }
    }
}
