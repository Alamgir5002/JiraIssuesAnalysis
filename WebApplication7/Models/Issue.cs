using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApplication7.Models
{
    public class Issue
    {
        [Key]
        public string Id { get; set; }
        public IssueType IssueType { get; set; }
        public string IssueTypeId { get; set; }
        public ICollection<IssueRelease> FixVersions { get; set; }
        public EstimatedAndSpentTime IssueEstimatedAndSpentTime { get; set; }
        public string? Summary {  get; set; }
        public string CreatedDate { get; set; }
        public string? ResolvedDate { get; set; }
        public string Priority { get; set; }    
        public int StoryPoints { get; set;}
        public string Status { get; set; }
        public Parent? Parent { get; set; }
        public string? ParentId { get; set; }
        public TeamBoard? TeamBoard { get; set; }
        public string? TeamBoardId { get; set; }
        public Uri IssueUrl { get; set; }
        public double ProductivityRatio { get; set; }
    }

    public class TeamBoard
    {
        [Key]
        public string Id { get; set; }  
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<Issue> IssuesList { get; set; }
    }

    public class Parent
    {
        [Key]
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
        public double AggregatedTimeSpent { get; set; }
        public double AggregateTimeEstimate { get; set; }
        public double AggregatedTimeSpentInDays { get; set; }
        public double AggregatedTimeEstimateInDays { get; set; }
        [JsonIgnore]
        public Issue Issue { get; set; }
        [Key]
        public string Id { get; set; }
    }

    public class IssueType
    {
        public string Name { get; set; }
        public bool SubTask { get; set; }
        [Key]
        public string Id { get; set; }
        [JsonIgnore]
        public ICollection<Issue> IssuesList { get; set; }
    }
}
