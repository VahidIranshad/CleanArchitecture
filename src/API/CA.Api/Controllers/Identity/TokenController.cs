using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;
using CA.Application.Features.Identity.Token.Queries;
using CA.Application.Features.Identity.User.Commands;
using CA.Identity.Utility;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CA.Api.Controllers.Identity
{
    [Route("api/identity/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<RoleController> _logger;

        public TokenController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult> Login([FromBody]TokenRequest model)
        {
            var pass = AESEncryptDecrypt.DecryptStringAES(model.Password);
            var email = AESEncryptDecrypt.DecryptStringAES(model.Email);
            model.Password = pass;
            model.Email = email;
            var response = await _mediator.Send(new LoginQuery() { Request = model });
            return Ok(response);
        }

        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh([FromBody] RefreshTokenRequest model)
        {
            var response = await _mediator.Send(new GetRefreshTokenQuery() { Request = model });
            return Ok(response);
        }
    }
}