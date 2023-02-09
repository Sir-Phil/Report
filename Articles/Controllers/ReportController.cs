using Articles.Services.Reports;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Articles.Controllers
{
    [Route("reports")]
    public class ReportController : Controller
    {
        private readonly IMediator _mediator;

        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ReportDTOs> Get([FromQuery] string tag,[FromQuery] string author, [FromQuery] string favorited, [FromQuery] int? limit, [FromQuery] int? offset, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new Fetch.Query(tag, author, favorited, limit, offset), cancellationToken);
        }


        [HttpPost] 
        public Task<ReportDTO> Create([FromBody] Create.Command command, CancellationToken cancellationToken)
        {
            return _mediator.Send(command, cancellationToken);
        }
    }
}
