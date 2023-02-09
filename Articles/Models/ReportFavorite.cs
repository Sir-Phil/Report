namespace Articles.Models
{
    public class ReportFavorite
    {
        public int ReportId { get; set; }
        public Report Report { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
