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

        /// <summary>
        /// Get Users Details
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.UsersPermissions.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _mediator.Send(new GetAllUsersQuery() {  });
            return Ok(users);
        }

        // GET: api/<AlternativeController>
        [HttpPost("GetList")]
        [Authorize(Policy = Permissions.UsersPermissions.View)]
        public async Task<ActionResult<(List<UserResponse>, int)>> GetList([FromBody] FopFilter filter)
        {
            var (list, totalCount) = await _mediator.Send(new GetListByFopFilterQuery() { filter = filter });
            return Ok(new ListByCount<UserResponse> { DataList = list, TotalCount = totalCount });
        }

        // GET: api/<AlternativeController>
        [HttpGet("GetByEmail")]
        [Authorize(Policy = Permissions.UsersPermissions.View)]
        public async Task<ActionResult<(UserResponse, int)>> GetByEmail(string email)
        {
            FopFilter filter = new FopFilter { Filter = $"Email=={email}" };
            var (list, totalCount) = await _mediator.Send(new GetListByFopFilterQuery() { filter = filter });
            return Ok(list.FirstOrDefault());
        }


        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Users.View)]
        [HttpGet("{id}")]
        [Authorize(Policy = Permissions.UsersPermissions.View)]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _mediator.Send(new GetUserByIDQuery() {UserID = id });
            return Ok(user);
        }

        /// <summary>
        /// Register a User
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            request.Password = AESEncryptDecrypt.DecryptStringAES(request.Password);
            request.ConfirmPassword = AESEncryptDecrypt.DecryptStringAES(request.ConfirmPassword);
            await _mediator.Send(new RegisterUserCommand() { Request = request , Origin = origin});
            return Ok();
        }

        /// <summary>
        /// Confirm Email
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns>Status 200 OK</returns>
        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            return Ok(await _mediator.Send(new ConfirmUserEmailQuery() { UserID = userId, Code = code }));
        }

        /// <summary>
        /// Toggle User Status (Activate and Deactivate)
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Status 200 OK</returns>
        [HttpPost("toggle-status")]
        public async Task<IActionResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
        {
            await _mediator.Send(new ToggleUserStatusCommand() { Request = request});
            return Ok();
        }

        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Status 200 OK</returns>
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var origin = Request.Headers["origin"];
            await _mediator.Send(new ForgotPasswordCommand() { Request = request, Origin = origin });
            return Ok();
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Status 200 OK</returns>
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            await _mediator.Send(new ResetPasswordCommand() { Request = request });
            return Ok();
        }

        /// <summary>
        /// Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.UsersPermissions.Export)]
        //[HttpGet("export")]
        //public async Task<IActionResult> Export(string searchString = "")
        //{
        //    var data = await _userService.ExportToExcelAsync(searchString);
        //    return Ok(data);
        //}
    }
}