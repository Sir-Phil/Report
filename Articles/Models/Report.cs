using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Articles.Models
{
    public class Report
    {
        [JsonIgnore]
        public int ReportId { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public Person Author { get; set; }
        public List<Comment> Comments { get; set; }
        [NotMapped]
        public bool Favorited => ReportFavorites?.Any() ?? false;
        [NotMapped]
        public int ReportCount => ReportFavorites?.Count ?? 0;
        [NotMapped]
        public List<string> TagList => ReportTags.Where(x => x.TagId is not null).Select(x => x.TagId!).ToList();
        [JsonIgnore]
        public List<ReportTag> ReportTags { get; set; } = new();
        [JsonIgnore]
        public List<ReportFavorite> ReportFavorites { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
