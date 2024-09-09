using Articles.Infrastructure;
using Articles.Infrastructure.Erros;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Articles.Controllers.Reports
{
    public class Details
    {
        public record Query(string Slug) : IRequest<ReportEnvelope>;

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Slug).NotNull().NotEmpty();
            }
        }

        public class QueryHandler : IRequestHandler<Query, ReportEnvelope>
        {
            private readonly ReportDbContext _reportDbContext;

            public QueryHandler(ReportDbContext reportDbContext)
            {
                _reportDbContext = reportDbContext;
            }
            public async Task<ReportEnvelope> Handle(Query message, CancellationToken cancellationToken)
            {
                var report = await _reportDbContext.Reports.GetAllData().FirstOrDefaultAsync
                    (x => x.Slug == message.Slug, cancellationToken);

                if (report == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new
                    {
                        Report = Constants.NOT_FOUND
                    });
                }
                return new ReportEnvelope(report);
            }
        }
    }
}
