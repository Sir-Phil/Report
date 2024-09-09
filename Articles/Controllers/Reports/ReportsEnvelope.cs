using Articles.Models;

namespace Articles.Controllers.Reports
{
    public class ReportsEnvelope
    {
        public List<Report> Reports { get; set; } = new();

        public int ReportCount { get; set; }

    }
}
