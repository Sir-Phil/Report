using Articles.Infrastructure;
using Articles.Infrastructure.CurrentUser;
using Articles.Infrastructure.Security;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Articles.Controllers.Users
{
    public class Edit
    {
        public class UserData
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Bio { get; set; }
            public string Image { get; set; }
        }

        public record Command(UserData User) : IRequest<UserEnvelope>;
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.User).NotNull();
            }
        }
        public class Handler : IRequestHandler<Command, UserEnvelope>
        {
            private readonly ReportDbContext _reporDbContext;
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly IPasswordHasher _passwordHasher;
            private readonly IMapper _mapper;

            public Handler(ReportDbContext reporDbContext,
                ICurrentUserAccessor currentUserAccessor,
                IPasswordHasher passwordHasher,
                IMapper mapper
                )
            {
                _reporDbContext = reporDbContext;
                _currentUserAccessor = currentUserAccessor;
                _passwordHasher = passwordHasher;
                _mapper = mapper;
            }
            public async Task<UserEnvelope> Handle(Command mesaage, CancellationToken cancellationToken)
            {
                var currentUserName = _currentUserAccessor.GetCurrentUserName();
                var person = await _reporDbContext.Persons.Where(x => x.Username == currentUserName).FirstOrDefaultAsync(cancellationToken);

                person.Username = mesaage.User.Username ?? person.Username;
                person.Email = mesaage.User.Email ?? person.Email;
                person.Bio = mesaage.User.Bio ?? person.Bio;
                person.Image = mesaage.User.Image ?? person.Image;

                if (!string.IsNullOrWhiteSpace(mesaage.User.Password))
                {
                    var salt = Guid.NewGuid().ToByteArray();
                    person.Hash = await _passwordHasher.Hash(mesaage.User.Password, salt);
                    person.Salt = salt;
                }

                await _reporDbContext.SaveChangesAsync(cancellationToken);

                return new UserEnvelope(_mapper.Map<Models.Person, User>(person));
            }
        }
    }
}
