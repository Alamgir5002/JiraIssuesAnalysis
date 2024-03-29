using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApplication7.Models
{
    public class Release
    {
        [Key]
        public string Id { get; set; }  
        public string Name { get; set; }
        public bool Released { get; set;}
        public string? ReleaseDate { get; set; }
        [JsonIgnore]
        public ICollection<IssueRelease> IssueReleases { get; set; }
    }

    public class IssueRelease
    {
        public string IssueId { get; set; }
        [JsonIgnore]
        public Issue Issue { get; set; } // Navigation property
        public string ReleaseId { get; set; }
        public Release Release { get; set; } // Navigation property
    }
}
