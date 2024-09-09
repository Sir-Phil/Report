using Articles.Models;
using Microsoft.EntityFrameworkCore;

namespace Articles.Controllers.Reports
{
    public static class ReportExtensions
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
