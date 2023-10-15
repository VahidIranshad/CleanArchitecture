using CA.Application.DTOs.Ent;
using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;
using CA.Application.Features.Ent.Selection.Queries;
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

        [ProducesResponseType(typeof(List<RoleResponse>), StatusCodes.Status200OK)]
        [Authorize(Policy = Permissions.RolesPermissions.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _mediator.Send(new GetAllRolesQuery());
            return Ok(roles);
        }

        [ProducesResponseType(typeof(List<RoleResponse>), StatusCodes.Status200OK)]
        [HttpGet("GetList")]
        [Authorize(Policy = Permissions.RolesPermissions.View)]
        public async Task<IActionResult> GetList([FromQuery] FopFilter query)
        {
            var (list, totalCount) = await _mediator.Send(new GetRolesByFopFilterQuery() { filter = query });
            return Ok(new ListByCount<RoleResponse> { DataList = list, TotalCount = totalCount });
        }


        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[Authorize(Policy = Permissions.RolesPermissions.Create)]
        //[HttpPost]
        //public async Task<IActionResult> Post(RoleRequest request)
        //{
        //    //var response = await _roleService.SaveAsync(request);
        //    //return Ok(response);
        //    return NoContent();
        //}

        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[Authorize(Policy = Permissions.RolesPermissions.Delete)]
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    //var response = await _roleService.DeleteAsync(id);
        //    //return Ok(response);
        //    return NoContent();
        //}


        //[ProducesResponseType(typeof(List<PermissionResponse>), StatusCodes.Status200OK)]
        //[Authorize(Policy = Permissions.RoleClaimsPermissions.View)]
        //[HttpGet("permissions/{roleId}")]
        //public async Task<IActionResult> GetPermissionsByRoleId([FromRoute] string roleId)
        //{
        //    var response = await _mediator.Send(new GetAllPermissionsQuery() { roleId = roleId });
        //    return Ok(response);
        //}

        //[Authorize(Policy = Permissions.RoleClaimsPermissions.Edit)]
        //[HttpPut("permissions/update")]
        //public async Task<IActionResult> Update(PermissionRequest model)
        //{
        //    //var response = await _roleService.UpdatePermissionsAsync(model);
        //    //return Ok(response);
        //    return Ok();
        //}
    }
}