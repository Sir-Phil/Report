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

        [HttpGet("feed")]
        public Task<ReportDTOs> GetFeed([FromQuery] string tag, [FromQuery] string author, [FromQuery] string favorited, [FromQuery] int? limit, [FromQuery] int? offset, CancellationToken cancellationToken)
        {
            return _mediator.Send(new Fetch.Query(tag, author, favorited, limit, offset)
            {
                iSFeed = true
            });
        }

        [HttpGet("{slug}")]
        public Task<ReportDTO> Get(string slug, CancellationToken cancellationToken)
        {
            return _mediator.Send(new Details.Query(slug), cancellationToken);
        }

        [HttpPost] 
        public Task<ReportDTO> Create([FromBody] Create.Command command, CancellationToken cancellationToken)
        {
            return _mediator.Send(command, cancellationToken);
        }

        [HttpPut("{slug}")]
        public Task<ReportDTO> Edit(string slug, [FromBody] Edit.Model model, CancellationToken cancellationToken)
        {
            return _mediator.Send(new Edit.Command(model, slug), cancellationToken);
        }

        [HttpDelete("{slug}")]
        public Task Delete(string slug, CancellationToken cancellationToken)
        {
            return _mediator.Send(new Delete.Command(slug), cancellationToken);
        }
    }
}
