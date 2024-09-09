using Articles.Infrastructure;
using Articles.Infrastructure.Erros;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Articles.Controllers.Reports
{
    public class Delete
    {
        public record Command(string Slug) : IRequest;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Slug).NotNull().NotEmpty();
            }
        }

        public class QueryHandler : IRequestHandler<Command>
        {
            private readonly ReportDbContext _reportDbContext;

            public QueryHandler(ReportDbContext reportDbContext)
            {
                _reportDbContext = reportDbContext;
            }

            public async Task<Unit> Handle(Command message, CancellationToken cancellationToken)
            {
                var report = await _reportDbContext.Reports.FirstOrDefaultAsync
                    (x => x.Slug == message.Slug, cancellationToken);

                if (report == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new
                    {
                        Report = Constants.NOT_FOUND
                    });
                }

                _reportDbContext.Reports.Remove(report);
                await _reportDbContext.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
