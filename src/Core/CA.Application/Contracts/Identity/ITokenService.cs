using CA.Application.Contracts.Base;
using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;
using System.Security.Claims;

namespace CA.Application.Contracts.Identity
{
    public interface ITokenService : IService
    {
        Task<TokenResponse> LoginAsync(TokenRequest model);

        Task<TokenResponse> GetRefreshTokenAsync(RefreshTokenRequest model);

        Task<IEnumerable<Claim>> GetClaimsAsync(string userID);
    }
}