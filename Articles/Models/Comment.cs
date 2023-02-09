using System.Text.Json.Serialization;

namespace Articles.Models
{
    public class Comment
    {
        [JsonPropertyName("id")]
        public int CommentId { get; set; }
        public string Body { get; set; }
        public Person Author { get; set; }
        [JsonIgnore]
        public int AuthorId { get; set; }
        [JsonIgnore]
        public Report Report { get; set; }
        [JsonIgnore]
        public int ReportId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}