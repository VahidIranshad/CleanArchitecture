using CA.Application.DTOs.Ent;
using CA.Application.Features.Ent.Selection.Queries;
using CA.Domain.Base;
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
    public class SelectionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<SelectionController> _logger;

        public SelectionController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<SelectionDto>>> GetAll()
        {
            var list = await _mediator.Send(new GetListSelectionQuery());
            return Ok(list);
        }


        [HttpPost("GetList")]
        public async Task<ActionResult<(List<SelectionDto>, int)>> GetList([FromBody] FopFilter filter)
        {
            var (list, totalCount) = await _mediator.Send(new GetListSelectionQuery() { filter = filter });
            return Ok(new ListByCount<SelectionDto> { DataList = list, TotalCount = totalCount });
        }


    }
}
