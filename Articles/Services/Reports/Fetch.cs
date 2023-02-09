﻿using Articles.Infrastructure;
using Articles.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Articles.Services.Reports
{
    public class Fetch
    {
        public record Query(
            string Tag,string Author, string FavoritesUsername, int? Limit, int? Offset,
            bool iSFeed = false) : IRequest<ReportDTOs>;

        public class QueryHandler : IRequestHandler<Query, ReportDTOs>
        {
            private readonly ReportDbContext _reportDbContext;

            public QueryHandler(ReportDbContext reportDbContext)
            {
                _reportDbContext = reportDbContext;
            }

            public async Task<ReportDTOs> Handle(Query message, CancellationToken cancellationToken)
            {
                IQueryable<Report> queryable = _reportDbContext.Reports.GetAllData();

                if (!string.IsNullOrEmpty(message.Tag))
                {
                    var tag = await _reportDbContext.ReportTags.FirstOrDefaultAsync(x => x.TagId == message.Tag, cancellationToken);
                    if (tag != null)
                    {
                        queryable = queryable.Where(x => x.ReportTags.Select(y => y.TagId).Contains(tag.TagId));
                    }
                    else
                    {
                        return new ReportDTOs();
                    }
                }

                if (!string.IsNullOrWhiteSpace(message.Author))
                {
                    var author = await _reportDbContext.Persons.FirstOrDefaultAsync(x => x.Username == message.Author, cancellationToken);
                    if(author != null)
                    {
                        queryable = queryable.Where(x => x.Author == author);
                    }
                    else
                    {
                        return new ReportDTOs();
                    }
                }

                if (!string.IsNullOrWhiteSpace(message.FavoritesUsername)) 
                {
                    var author = await _reportDbContext.Persons.FirstOrDefaultAsync(x => x.Username == message.FavoritesUsername , cancellationToken);
                    if (author != null)
                    {
                        queryable = queryable.Where(x => x.ReportFavorites.Any(y => y.PersonId == author.PersonId));
                    }
                    else
                    {
                        return new ReportDTOs();
                    }

                }

                var report = await queryable
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip(message.Offset ?? 0)
                    .Take(message.Limit ?? 0)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                return new ReportDTOs()
                {
                    Reports = report,
                    ReportCount = queryable.Count()
                };

            }
        }
    }
}
