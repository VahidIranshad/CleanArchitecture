using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Requests;
using MediatR;
using CA.Application.Features.Identity.Account.Commands;

namespace CA.Api.Controllers.Identity
{
    [Authorize]
    [Route("api/identity/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AccountController> _logger;
        private readonly ICurrentUserService _currentUser;

        public AccountController(IMediator mediator, IHttpContextAccessor httpContextAccessor, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
            _currentUser = currentUserService;
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut(nameof(UpdateProfile))]
        public async Task<ActionResult> UpdateProfile(UpdateProfileRequest model)
        {
            await _mediator.Send(new UpdateProfileCommand { model = model, userId = _currentUser.UserId });
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut(nameof(ChangePassword))]
        public async Task<ActionResult> ChangePassword(ChangePasswordRequest model)
        {
            await _mediator.Send(new ChangePasswordCommand { model = model, userId = _currentUser.UserId });
            return NoContent();
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("profile-picture/{userId}")]
        [ResponseCache(NoStore = false, Location = ResponseCacheLocation.Client, Duration = 60)]
        public async Task<IActionResult> GetProfilePictureAsync(string userId)
        {
            //return Ok(await _accountService.GetProfilePictureAsync(userId));
            return Ok();
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("profile-picture/{userId}")]
        public async Task<IActionResult> UpdateProfilePictureAsync(UpdateProfilePictureRequest request)
        {
            //await _accountService.UpdateProfilePictureAsync(request, _currentUser.UserId);
            return Ok();
        }
    }
}