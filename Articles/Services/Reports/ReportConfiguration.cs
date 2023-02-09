using Articles.Models;
using Microsoft.EntityFrameworkCore;

namespace Articles.Services.Reports
{
    public static class ReportConfiguration
    {
        public static IQueryable<Report> GetAllData(this DbSet<Report> reports)
        {
            return reports
                .Include(x => x.Author)
                .Include(x => x.ReportFavorites)
                .Include(x => x.ReportTags)
                .AsNoTracking();

        }
    }
}
