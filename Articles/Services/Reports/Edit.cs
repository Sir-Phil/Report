using Articles.Infrastructure;
using Articles.Infrastructure.Erros;
using Articles.Infrastructure.SlugConfig;
using Articles.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Articles.Services.Reports
{
    public class Edit
    {
        public class ReportDataEdit
        {
            public string Title { get; set; }
            public string Body { get; set; }
            public string Description { get; set; }
            public string[] TagList { get; set; }
        }

        public record Command(Model Model, string Slug) : IRequest<ReportDTO>;

        public record Model(ReportDataEdit Report);

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Model.Report).NotNull();
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
                var report = await _reportDbContext.Reports
                    .Include(x => x.ReportTags) // includes reports tags since they need to be updated
                    .Where(x => x.Slug == message.Slug)
                    .FirstOrDefaultAsync(cancellationToken);

                if(report == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Report = Constants.NOT_FOUND });
                }

                report.Description = message.Model.Report.Description ?? report.Description;
                report.Body = message.Model.Report.Body ?? report.Body;
                report.Title = message.Model.Report.Title ?? report.Title;
                report.Slug = report.Title.GenerateSlug();

                // list of currently save report tag for the given report
                var reportTagList = (message.Model.Report.TagList ?? Enumerable.Empty<string>());
                var reportTagsToCreate = GetReportTagsToCreate(report, reportTagList);
                var reportTagsToDelete = GetReportTagsToDelete(report, reportTagList);

                if (_reportDbContext.ChangeTracker.Entries().First(x => x.Entity == report).State == EntityState.Modified
                    || reportTagsToCreate.Any() 
                    || reportTagsToDelete.Any()
                    )
                {
                    report.UpdatedAt = DateTime.UtcNow;
                }

                // ensure context is treacking any tags that are about to be created so that it won't attepmt to insert a duplicate
                _reportDbContext.Tags.AttachRange(reportTagsToCreate.Where(x => x.Tag is not null)
                    .Select(a => a.Tag!)
                    .ToArray());

                // Add the new report tags
                await _reportDbContext.ReportTags.AddRangeAsync(reportTagsToCreate, cancellationToken);

                // delete the tags that does not exists anymore
                _reportDbContext.ReportTags.RemoveRange(reportTagsToDelete);
                await _reportDbContext.SaveChangesAsync(cancellationToken);

                return new ReportDTO(await _reportDbContext.Reports.GetAllData()
                    .Where(x => x.Slug == report.Slug)
                    .FirstOrDefaultAsync(cancellationToken)
                    );
            }

            // check which report tag needs to be deleted
            static List<ReportTag> GetReportTagsToDelete(Report report, IEnumerable<string> reportTagList)
            {
                var reportTagsToDelete = new List<ReportTag>();
                foreach(var tag in report.ReportTags)
                {
                    var rt = reportTagList.FirstOrDefault(t => t == tag.TagId);
                    if (rt == null)
                    {
                        reportTagsToDelete.Add(tag);
                    }
                }
                return reportTagsToDelete;
            }

            // check which report tag needs to be added
            static List<ReportTag> GetReportTagsToCreate(Report report, IEnumerable<string> reportTagList)
            {
                var reportTagsToCreate = new List<ReportTag>();
                foreach(var tag in reportTagList)
                {
                    var rt = report.ReportTags?.FirstOrDefault(t => t.TagId == tag);
                    if (rt == null)
                    {
                        rt = new ReportTag()
                        {
                            Report = report,
                            ReportId = report.ReportId,
                            Tag = new Tag() { TagId = tag },
                            TagId = tag
                        };
                        reportTagsToCreate.Add(rt);
                    }
                }
                return reportTagsToCreate;
            }
        }
    }
}
