using Articles.Services.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Articles.Controllers
{
    [Route("users")]
    public class UsersController 
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public Task<UserDTO> Create([FromBody] Create.Command command, CancellationToken cancellationToken)
        {
            return _mediator.Send(command, cancellationToken);
        }

        [HttpPost]
        public Task<UserDTO> Login([FromBody] Login.Command command, CancellationToken cancellationToken)
        {
            return _mediator.Send(command, cancellationToken);
        }
    }
}
