using CA.Application.DTOs.Ent.TValue;
using CA.Application.Features.Generic.Commands;
using CA.Application.Features.Generic.Queries;
using CA.Domain.Base;
using CA.Domain.Constants.Permission;
using CA.Domain.Ent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CA.Api.Controllers.Ent
{
    [ApiController]
    [Route("api/ent/[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    [Authorize]
    public class TValueController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TValueController> _logger;

        public TValueController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }


        [ProducesResponseType(typeof(List<TValueDto>), StatusCodes.Status200OK)]
        [HttpGet("GetAll")]
        [Authorize(Policy = Permissions.TValuePermissions.View)]
        public async Task<IActionResult> GetAll()
        {
            var list = await _mediator.Send(new GetListBaseQuery<TValueDto, TValue>());
            return Ok(list);
        }


        [ProducesResponseType(typeof((List<TValueDto>, int)), StatusCodes.Status200OK)]
        [HttpGet("GetList")]
        [Authorize(Policy = Permissions.TValuePermissions.View)]
        public async Task<IActionResult> GetList([FromQuery] FopFilter query)
        {
            var (list, totalCount) = await _mediator.Send(new GetListByFopFilterQuery<TValueDto, TValue>() { filter = query });
            return Ok(new ListByCount<TValueDto> { DataList = list, TotalCount = totalCount });
        }



        [ProducesResponseType(typeof(TValueDto), StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        [Authorize(Policy = Permissions.TValuePermissions.View)]
        public async Task<ActionResult<TValueDto>> Get(int id)
        {
            var data = await _mediator.Send(new GetDetailBaseQuery<TValueDto, TValue> { id = id });
            return Ok(data);
        }


        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [Authorize(Policy = Permissions.TValuePermissions.Create)]
        public async Task<ActionResult> Create([FromBody] TValueDto data)
        {
            var command = new CreateBaseCommand<TValue> { CreateBaseDto = data };
            var id = await _mediator.Send(command);
            return Ok(id);
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        [Authorize(Policy = Permissions.TValuePermissions.Edit)]
        public async Task<ActionResult> Update([FromBody] TValueDto data)
        {
            var command = new UpdateBaseCommand<TValue> { UpdateBaseDto = data };
            await _mediator.Send(command);
            return NoContent();
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        [Authorize(Policy = Permissions.TValuePermissions.Delete)]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteBaseCommand<TValue> { DeleteBaseDto = new TValueDto() { Id = id } };
            await _mediator.Send(command);
            return NoContent();
        }


    }
}
