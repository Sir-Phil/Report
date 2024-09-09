using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Articles.Controllers.Users
{
    [Route("users")]
    public class UsersController
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public Task<UserEnvelope> Create([FromBody] Create.Command command, CancellationToken cancellationToken)
        {
            return _mediator.Send(command, cancellationToken);
        }

        [HttpPost("login")]
        public Task<UserEnvelope> Login([FromBody] Login.Command command, CancellationToken cancellationToken)
        {
            return _mediator.Send(command, cancellationToken);
        }
    }
}
