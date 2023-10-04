using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;
using CA.Application.Features.Identity.RoleClaim.Commands;
using CA.Application.Features.Identity.RoleClaim.Queries;
using CA.Domain.Base;
using CA.Domain.Constants.Permission;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CA.Api.Controllers.Identity
{
    [Route("api/identity/roleClaim")]
    [ApiController]
    public class RoleClaimController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<RoleClaimController> _logger;

        public RoleClaimController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<RoleClaimResponse>), StatusCodes.Status200OK)]
        [Authorize(Policy = Permissions.RoleClaimsPermissions.View)]
        [HttpGet]
        public async Task<ActionResult<List<RoleClaimResponse>>> GetAll()
        {
            var roleClaims = await _mediator.Send(new GetAllRoleClaimsQuery());
            return Ok(roleClaims);
        }


        [ProducesResponseType(typeof(List<RoleClaimResponse>), StatusCodes.Status200OK)]
        [Authorize(Policy = Permissions.RoleClaimsPermissions.View)]
        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetAllByRoleId(string roleId)
        {
            var response = await _mediator.Send(new GetAllRoleClaimsByRoleIdQuery() { roleId = roleId });
            return Ok(response);
        }

        [ProducesResponseType(typeof(List<Claims>), StatusCodes.Status200OK)]
        [Authorize(Policy = Permissions.RoleClaimsPermissions.View)]
        [HttpGet("GetAllClaims")]
        public async Task<IActionResult> GetAllClaims()
        {
            var response = CA.Domain.Constants.Permission.Permissions.AllPermision.GetAllPermision();
            return Ok(response);
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = Permissions.RoleClaimsPermissions.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(RoleClaimRequest request)
        {

            await _mediator.Send(new InsertRoleClaimCommand() { Model = request});
            return NoContent();
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = Permissions.RoleClaimsPermissions.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteRoleClaimCommand() { Id = id });
            return NoContent();
        }
    }
}