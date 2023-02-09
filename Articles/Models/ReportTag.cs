namespace Articles.Models
{
    public class ReportTag
    {
        public int ReportId { get; set; }
        public Report Report { get; set; }
        public string TagId { get; set; }
        public Tag Tag { get; set; }
    }
}