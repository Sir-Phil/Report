using Articles.Infrastructure;
using Articles.Infrastructure.SlugConfig;
using Articles.Models;
using FluentValidation;
using MediatR;

namespace Articles.Services.Reports
{
    public class Create
    {
        public class ReportData
        {
            public string Title { get; set; }
            public string Body { get; set; }
            public string  Description { get; set; }
            public string[] TagList { get; set; }
        }

        public class ReportDataValiadator : AbstractValidator<ReportData>
        {
            public ReportDataValiadator()
            {
                RuleFor(x => x.Title).NotEmpty();
                RuleFor(x => x.Body).NotEmpty();
                RuleFor(x =>x.Description).NotEmpty();
            }
        }

        public record Command(ReportData Report): IRequest<ReportDTO>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Report).NotNull().SetValidator(new ReportDataValiadator());
            }
        }
        public class Handler : IRequestHandler<Command, ReportDTO>
        {
            private readonly ReportDbContext _reportDbContext;

            public Handler(ReportDbContext reportDbContext)
            {
                _reportDbContext = reportDbContext;
            }
            public async Task<ReportDTO> Handle(Command message, CancellationToken cancellationToken)
            {
                var author = await _reportDbContext.Persons.FindAsync(cancellationToken);
                var tags = new List<Tag>();
                foreach (var tag in(message.Report.TagList ?? Enumerable.Empty<string>()))
                {
                    var t = await _reportDbContext.Tags.FindAsync(tag);
                    if (t == null)
                    {
                        t = new Tag()
                        {
                            TagId = tag
                        };
                        await _reportDbContext.Tags.AddAsync(t, cancellationToken);
                        // save immediately for reuse
                        await _reportDbContext.SaveChangesAsync(cancellationToken);
                    }
                    tags.Add(t);
                }

                var report = new Report()
                {
                    Author = author,
                    Title = message.Report.Title,
                    Body = message.Report.Body,
                    Description = message.Report.Description,
                    Slug = message.Report.Title.GenerateSlug()
                };
                await _reportDbContext.AddAsync(report, cancellationToken);

                await _reportDbContext.ReportTags.AddRangeAsync(tags.Select(x => new ReportTag()
                {
                    Report = report,
                    Tag = x
                }), cancellationToken);

                await _reportDbContext.SaveChangesAsync(cancellationToken);
                return new ReportDTO(report);
            }
        }
    }
}
