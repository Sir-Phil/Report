using Articles.Models;

namespace Articles.Services.Reports
{
    public class ReportDTOs
    {
        public List<Report> Reports { get; set; } = new();

        public int ReportCount { get; set; }

    }
}
