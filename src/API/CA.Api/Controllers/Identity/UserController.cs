using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;
using CA.Application.Features.Identity.User.Commands;
using CA.Application.Features.Identity.User.Queries;
using CA.Domain.Base;
using CA.Domain.Constants.Permission;
using CA.Identity.Utility;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CA.Api.Controllers.Identity
{
    [Authorize]
    [Route("api/identity/user")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<RoleController> _logger;

        public UserController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        [ProducesResponseType(typeof(List<UserResponse>), StatusCodes.Status200OK)]
        [Authorize(Policy = Permissions.UsersPermissions.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _mediator.Send(new GetAllUsersQuery() {  });
            return Ok(users);
        }

        [ProducesResponseType(typeof(List<UserResponse>), StatusCodes.Status200OK)]
        [HttpGet("GetList")]
        [Authorize(Policy = Permissions.UsersPermissions.View)]
        public async Task<ActionResult<(List<UserResponse>, int)>> GetList([FromQuery] FopFilter query)
        {
            var (list, totalCount) = await _mediator.Send(new GetListByFopFilterQuery() { filter = query });
            return Ok(new ListByCount<UserResponse> { DataList = list, TotalCount = totalCount });
        }

        [ProducesResponseType(typeof((UserResponse, int)), StatusCodes.Status200OK)]
        [HttpGet("GetByEmail")]
        [Authorize(Policy = Permissions.UsersPermissions.View)]
        public async Task<ActionResult<(UserResponse, int)>> GetByEmail(string email)
        {
            FopFilter filter = new FopFilter { Filter = $"Email=={email}" };
            var (list, totalCount) = await _mediator.Send(new GetListByFopFilterQuery() { filter = filter });
            return Ok(list.FirstOrDefault());
        }


        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        [Authorize(Policy = Permissions.UsersPermissions.View)]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _mediator.Send(new GetUserByIDQuery() {UserID = id });
            return Ok(user);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            request.Password = AESEncryptDecrypt.DecryptStringAES(request.Password);
            request.ConfirmPassword = AESEncryptDecrypt.DecryptStringAES(request.ConfirmPassword);
            await _mediator.Send(new RegisterUserCommand() { Request = request , Origin = origin});
            return NoContent();
        }

        //[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        //[HttpGet("confirm-email")]
        //[AllowAnonymous]
        //public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        //{
        //    return Ok(await _mediator.Send(new ConfirmUserEmailQuery() { UserID = userId, Code = code }));
        //}

        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[HttpPost("toggle-status")]
        //public async Task<IActionResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
        //{
        //    await _mediator.Send(new ToggleUserStatusCommand() { Request = request});
        //    return NoContent();
        //}

        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[HttpPost("forgot-password")]
        //[AllowAnonymous]
        //public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        //{
        //    var origin = Request.Headers["origin"];
        //    await _mediator.Send(new ForgotPasswordCommand() { Request = request, Origin = origin });
        //    return NoContent();
        //}

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            await _mediator.Send(new ResetPasswordCommand() { Request = request });
            return NoContent();
        }

        //[Authorize(Policy = Permissions.UsersPermissions.Export)]
        //[HttpGet("export")]
        //public async Task<IActionResult> Export(string searchString = "")
        //{
        //    var data = await _userService.ExportToExcelAsync(searchString);
        //    return Ok(data);
        //}
    }
}