
using Articles.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Articles.Controllers.Reports
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
        public async Task<ReportsEnvelope> Get([FromQuery] string tag, [FromQuery] string author, [FromQuery] string favorited, [FromQuery] int? limit, [FromQuery] int? offset, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new Fetch.Query(tag, author, favorited, limit, offset), cancellationToken);
        }

        [HttpGet("feed")]
        public Task<ReportsEnvelope> GetFeed([FromQuery] string tag, [FromQuery] string author, [FromQuery] string favorited, [FromQuery] int? limit, [FromQuery] int? offset, CancellationToken cancellationToken)
        {
            return _mediator.Send(new Fetch.Query(tag, author, favorited, limit, offset)
            {
                iSFeed = true
            });
        }

        [HttpGet("{slug}")]
        public Task<ReportEnvelope> Get(string slug, CancellationToken cancellationToken)
        {
            return _mediator.Send(new Details.Query(slug), cancellationToken);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtIssueOptions.Schemes)]
        public Task<ReportEnvelope> Create([FromBody] Create.Command command, CancellationToken cancellationToken)
        {
            return _mediator.Send(command, cancellationToken);
        }

        [HttpPut("{slug}")]
        [Authorize(AuthenticationSchemes =JwtIssueOptions.Schemes)]
        public Task<ReportEnvelope> Edit(string slug, [FromBody] Edit.Model model, CancellationToken cancellationToken)
        {
            return _mediator.Send(new Edit.Command(model, slug), cancellationToken);
        }

        [HttpDelete("{slug}")]
        [Authorize(AuthenticationSchemes =JwtIssueOptions.Schemes)]
        public Task Delete(string slug, CancellationToken cancellationToken)
        {
            return _mediator.Send(new Delete.Command(slug), cancellationToken);
        }
    }
}
