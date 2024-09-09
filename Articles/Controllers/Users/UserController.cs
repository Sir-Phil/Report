using Articles.Infrastructure.CurrentUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Articles.Controllers.Users
{
    [Route("user")]
    public class UserController
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public UserController(IMediator mediator, ICurrentUserAccessor currentUserAccessor)
        {
            _mediator = mediator;
            _currentUserAccessor = currentUserAccessor;
        }

        [HttpGet]
        public Task<UserEnvelope> GetCurrent(CancellationToken cancellationToken)
        {
            return _mediator.Send(new Details.Query(_currentUserAccessor.GetCurrentUserName() ?? "unkown"));
        }

        [HttpPut]
        public Task<UserEnvelope> UpdateUser([FromBody] Edit.Command command, CancellationToken cancellationToken)
        {
            return _mediator.Send(command, cancellationToken);
        }
    }
}
