using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApplication7.Models
{
    public class Issue
    {
        public string Id { get; set; }
        public IssueType IssueType { get; set; }
        public int IssueTypeId { get; set; }
        public ICollection<IssueRelease> FixVersions { get; set; }
        public EstimatedAndSpentTime IssueEstimatedAndSpentTime { get; set; }
        public string? Summary {  get; set; }
        public string CreatedDate { get; set; }
        public string? ResolvedDate { get; set; }
        public string Priority { get; set; }    
        public int StoryPoints { get; set;}
        public string Status { get; set; }
        public Parent? Parent { get; set; }
        public int? ParentId { get; set; }
        public TeamBoard? TeamBoard { get; set; }
        public int? TeamBoardId { get; set; }
        public Uri IssueUrl { get; set; }
        public double ProductivityRatio { get; set; }
    }

    public class TeamBoard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamBoardId { get; set; }
        public string Id { get; set; }  
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<Issue> IssuesList { get; set; }
    }

    public class Parent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public int ParentId { get; set; }
        public string Id { get; set; }
        public string Summary { get; set; }
        public Uri ParentUrl { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        [JsonIgnore]
        public ICollection<Issue> ChildIssues { get; set; }
    }

    public class EstimatedAndSpentTime
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double AggregatedTimeSpent { get; set; }
        public double AggregateTimeEstimate { get; set; }
        public int AggregatedTimeSpentInDays { get; set; }
        public int AggregatedTimeEstimateInDays { get; set; }
        [JsonIgnore]
        public Issue Issue { get; set; }
        public string IssueId { get; set; }
    }

    public class IssueType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IssueTypeId { get; set; }
        public string Name { get; set; }
        public bool SubTask { get; set; }
        public string Id { get; set; }
        [JsonIgnore]
        public ICollection<Issue> IssuesList { get; set; }
    }
}
