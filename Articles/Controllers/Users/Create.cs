using Articles.Infrastructure;
using Articles.Infrastructure.Erros;
using Articles.Infrastructure.Security;
using Articles.Models;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Articles.Controllers.Users
{
    public class Create
    {
        public class UserData
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public record Command(UserData User) : IRequest<UserEnvelope>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.User.Username).NotNull().NotEmpty();
                RuleFor(x => x.User.Email).NotNull().NotEmpty();
                RuleFor(x => x.User.Password).NotNull().NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Command, UserEnvelope>
        {
            private readonly ReportDbContext _reportDbContext;
            private readonly IMapper _mapper;
            private readonly IPasswordHasher _passwordHasher;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;

            public Handler(
                ReportDbContext reportDbContext,
                IMapper mapper,
                IPasswordHasher passwordHasher,
                IJwtTokenGenerator jwtTokenGenerator
                )
            {
                _reportDbContext = reportDbContext;
                _mapper = mapper;
                _passwordHasher = passwordHasher;
                _jwtTokenGenerator = jwtTokenGenerator;
            }
            public async Task<UserEnvelope> Handle(Command message, CancellationToken cancellationToken)
            {
                if (await _reportDbContext.Persons.Where(x => x.Username == message.User.Username).AnyAsync(cancellationToken))
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Username = Constants.IN_USE });
                }
                if (await _reportDbContext.Persons.Where(x => x.Email == message.User.Email).AnyAsync(cancellationToken))
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Email = Constants.IN_USE });
                }

                var salt = Guid.NewGuid().ToByteArray();
                var person = new Person
                {
                    Username = message.User.Username,
                    Email = message.User.Email,
                    Hash = await _passwordHasher.Hash(message.User.Password ?? throw new InvalidOperationException(), salt),
                    Salt = salt

                };
                await _reportDbContext.AddAsync(person, cancellationToken);
                await _reportDbContext.SaveChangesAsync(cancellationToken);

                var user = _mapper.Map<Person, User>(person);
                user.Token = _jwtTokenGenerator.CreateToken(person.Username ?? throw new InvalidOperationException());
                return new UserEnvelope(user);
            }
        }
    }
}
