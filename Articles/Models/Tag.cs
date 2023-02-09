namespace Articles.Models
{
    public class Tag
    {
        public string TagId { get; set; }
        public List<ReportTag> ReportTags { get; set; } = new();
    }
}