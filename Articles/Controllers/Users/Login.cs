using Articles.Infrastructure;
using Articles.Infrastructure.Erros;
using Articles.Infrastructure.Security;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Articles.Controllers.Users
{
    public class Login
    {
        public class UserDataLogin
        {
            public string Email { get; set; }
            public string Password { get; set; }

        }

        public record Command(UserDataLogin User) : IRequest<UserEnvelope>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.User).NotNull();
                RuleFor(x => x.User.Email).NotNull().NotEmpty();
                RuleFor(x => x.User.Password).NotNull().NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Command, UserEnvelope>
        {
            private readonly ReportDbContext _reportDbContext;
            private readonly IMapper _mapper;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;
            private readonly IPasswordHasher _passwordHasher;

            public Handler(
                ReportDbContext reportDbContext,
                IMapper mapper,
                IJwtTokenGenerator jwtTokenGenerator,
                IPasswordHasher passwordHasher
                )
            {
                _reportDbContext = reportDbContext;
                _mapper = mapper;
                _jwtTokenGenerator = jwtTokenGenerator;
                _passwordHasher = passwordHasher;
            }
            public async Task<UserEnvelope> Handle(Command message, CancellationToken cancellationToken)
            {
                var person = await _reportDbContext.Persons.Where(x => x.Email == message.User.Email).SingleOrDefaultAsync(cancellationToken);
                if (person == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid Email / Password." });
                }

                if (!person.Hash.SequenceEqual(await _passwordHasher.Hash(message.User.Password ?? throw new InvalidOperationException(), person.Salt)))
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid Email / Password" });
                }

                var user = _mapper.Map<Models.Person, User>(person);
                user.Token = _jwtTokenGenerator.CreateToken(person.Username ?? throw new InvalidOperationException());
                return new UserEnvelope(user);
            }
        }
    }
}
