using Articles.Infrastructure;
using Articles.Infrastructure.Erros;
using Articles.Infrastructure.Security;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Articles.Controllers.Users
{
    public class Details
    {
        public record Query(string Username) : IRequest<UserEnvelope>;

        public class QueryHandler : IRequestHandler<Query, UserEnvelope>
        {
            private readonly ReportDbContext _reportDbContext;
            private readonly IMapper _mapper;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;


            public QueryHandler(
                ReportDbContext reportDbContext,
                IMapper mapper,
                IJwtTokenGenerator jwtTokenGenerator
                )
            {
                _reportDbContext = reportDbContext;
                _mapper = mapper;
                _jwtTokenGenerator = jwtTokenGenerator;
            }
            public async Task<UserEnvelope> Handle(Query message, CancellationToken cancellationToken)
            {
                var person = await _reportDbContext.Persons.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Username == message.Username, cancellationToken);

                if (person == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { User = Constants.NOT_FOUND });
                }

                var user = _mapper.Map<Models.Person, User>(person);
                user.Token = _jwtTokenGenerator.CreateToken(person.Username ?? throw new InvalidOperationException());
                return new UserEnvelope(user);
            }
        }
    }
}
