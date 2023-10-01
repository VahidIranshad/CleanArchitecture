using CA.Application.DTOs.Identity.Requests;
using CA.Application.Features.Identity.RoleClaim.Commands;
using CA.Application.Features.Identity.RoleClaim.Queries;
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

        /// <summary>
        /// Get All Role Claims(e.g. Alternative Create Permission)
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.RoleClaimsPermissions.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roleClaims = await _mediator.Send(new GetAllRoleClaimsQuery());
            return Ok(roleClaims);
        }

        /// <summary>
        /// Get All Role Claims By Id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.RoleClaimsPermissions.View)]
        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetAllByRoleId(string roleId)
        {
            var response = await _mediator.Send(new GetAllRoleClaimsByRoleIdQuery() { roleId = roleId });
            return Ok(response);
        }

        [Authorize(Policy = Permissions.RoleClaimsPermissions.View)]
        [HttpGet("GetAllClaims")]
        public async Task<IActionResult> GetAllClaims()
        {
            var response = CA.Domain.Constants.Permission.Permissions.AllPermision.GetAllPermision();
            return Ok(response);
        }

        /// <summary>
        /// Add a Role Claim
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Status 200 OK </returns>
        [Authorize(Policy = Permissions.RoleClaimsPermissions.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(RoleClaimRequest request)
        {

            await _mediator.Send(new InsertRoleClaimCommand() { Model = request});
            return Ok();
        }

        /// <summary>
        /// Delete a Role Claim
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.RoleClaimsPermissions.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteRoleClaimCommand() { Id = id });
            return Ok();
        }
    }
}