using CA.Application.Contracts.Identity;
using MediatR;
using System.Security.Claims;

namespace CA.Application.Features.Identity.Token.Queries
{
    public class GetClaimsQuery : IRequest<IEnumerable<Claim>>
    {
        public string userID { get; set; }
    }
    public class GetClaimsQueryHandler : IRequestHandler<GetClaimsQuery, IEnumerable<Claim>>
    {

        private readonly ITokenService _tokenService;

        public GetClaimsQueryHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        public async Task<IEnumerable<Claim>> Handle(GetClaimsQuery request, CancellationToken cancellationToken)
        {
            return await _tokenService.GetClaimsAsync(request.userID);
        }
    }
}
