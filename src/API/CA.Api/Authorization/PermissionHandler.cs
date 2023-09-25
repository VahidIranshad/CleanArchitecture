using CA.Application.Contracts.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;
namespace CA.Api.Authorization
{
    public class PermissionHandler : IAuthorizationHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;
        private readonly IRoleClaimService _roleClaimService;
        public PermissionHandler(IHttpContextAccessor httpContextAccessor, IRoleClaimService roleClaimService)
        {

            _httpContextAccessor = httpContextAccessor;
            _roleClaimService = roleClaimService;
        }
        public Task HandleAsync(AuthorizationHandlerContext context)
        {

            if (context.User.Identity.IsAuthenticated == false)
            {
                return Task.CompletedTask;
            }
            if (context.HasSucceeded)
            {
                return Task.CompletedTask;
            }

            if (context.User == null)
            {
                return Task.CompletedTask;
            }
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            foreach (var requirement in context.Requirements)
            {
                if (requirement is ClaimsAuthorizationRequirement
                    && (requirement as ClaimsAuthorizationRequirement).ClaimType == "Permission"
                    && (requirement as ClaimsAuthorizationRequirement).AllowedValues.Any()
                    )
                {
                    var permissions = (requirement as ClaimsAuthorizationRequirement).AllowedValues.ToList();
                    var result = _roleClaimService.HasPermission(userId, permissions).Result;
                    if (result)
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
