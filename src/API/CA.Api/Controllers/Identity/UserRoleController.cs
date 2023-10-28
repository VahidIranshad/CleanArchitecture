using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;
using CA.Application.Features.Identity.User.Commands;
using CA.Application.Features.Identity.User.Queries;
using CA.Domain.Constants.Permission;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CA.Api.Controllers.Identity
{
    [Authorize]
    [Route("api/identity/userRole")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<RoleController> _logger;

        public UserRoleController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }


        [ProducesResponseType(typeof(List<UserRoleModel>), StatusCodes.Status200OK)]
        [Authorize(Policy = Permissions.UsersPermissions.View)]
        [Authorize(Policy = Permissions.UsersRolePermissions.View)]
        [HttpGet("{userID}")]
        public async Task<IActionResult> GetUserRolesByUserID(string userID)
        {
            var userRoles = await _mediator.Send(new GetUserRolesByUserIDQuery() { UserID = userID });
            return Ok(userRoles);
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = Permissions.UsersRolePermissions.Create)]
        [HttpPost("InsertUserRole")]
        public async Task<IActionResult> InsertUserRole(UpdateUserRolesRequest request)
        {
            await _mediator.Send(new InsertUserRoleCommand() { Request = request });
            return NoContent();
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = Permissions.UsersRolePermissions.Delete)]
        [HttpDelete("DeleteUserRole")]
        public async Task<IActionResult> DeleteUserRole([FromQuery] UpdateUserRolesRequest request)
        {
            await _mediator.Send(new DeleteUserRoleCommand() { Request = request });
            return NoContent();
        }

    }
}