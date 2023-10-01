using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;
using CA.Application.Features.Identity.Role.Queries;
using CA.Domain.Base;
using CA.Domain.Constants.Permission;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CA.Api.Controllers
{
    [Route("api/identity/role")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<RoleController> _logger;
        public RoleController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get All Roles (basic, admin etc.)
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.RolesPermissions.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _mediator.Send(new GetAllRolesQuery());
            return Ok(roles);
        }

        // GET: api/<AlternativeController>
        [HttpPost("GetList")]
        [Authorize(Policy = Permissions.UsersPermissions.View)]
        public async Task<ActionResult<(List<RoleResponse>, int)>> GetList([FromBody] FopFilter filter)
        {
            var (list, totalCount) = await _mediator.Send(new GetRolesByFopFilterQuery() { filter = filter });
            return Ok(new ListByCount<RoleResponse> { DataList = list, TotalCount = totalCount });
        }

        /// <summary>
        /// Add a Role
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.RolesPermissions.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(RoleRequest request)
        {
            //var response = await _roleService.SaveAsync(request);
            //return Ok(response);
            return Ok();
        }

        /// <summary>
        /// Delete a Role
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.RolesPermissions.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            //var response = await _roleService.DeleteAsync(id);
            //return Ok(response);
            return Ok();
        }

        /// <summary>
        /// Get Permissions By Role Id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.RoleClaimsPermissions.View)]
        [HttpGet("permissions/{roleId}")]
        public async Task<IActionResult> GetPermissionsByRoleId([FromRoute] string roleId)
        {
            var response = await _mediator.Send(new GetAllPermissionsQuery() { roleId = roleId });
            return Ok(response);
        }

        /// <summary>
        /// Edit a Role Claim
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.RoleClaimsPermissions.Edit)]
        [HttpPut("permissions/update")]
        public async Task<IActionResult> Update(PermissionRequest model)
        {
            //var response = await _roleService.UpdatePermissionsAsync(model);
            //return Ok(response);
            return Ok();
        }
    }
}