namespace WebApplication7.Models
{
    public class Issue
    {
        public string Id { get; set; }
        public IssueType IssueType { get; set; }
        public List<Release>? FixVersions { get; set; }
        public EstimatedAndSpentTime IssueEstimatedAndSpentTime { get; set; }
        public string? Summary {  get; set; }
        public string CreatedDate { get; set; }
        public string? ResolvedDate { get; set; }
        public string Priority { get; set; }    
        public int? StoryPoints { get; set;}
        public string Status { get; set; }
        public Parent? Parent { get; set; }
        public TeamBoard? TeamBoard { get; set; }
        public Uri? IssueUrl { get; set; }
    }

    public class TeamBoard
    {
        public string Id { get; set; }  
        public string Name { get; set; }
    }
    public class Parent
    {
        public string Id { get; set; }
        public string Summary { get; set; }
        public Uri ParentUrl { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string IssueType { get; set; }
    }

    public class EstimatedAndSpentTime
    {
        public double AggregatedTimeSpent { get; set; }
        public double AggregateTimeEstimate { get; set; }
        public int AggregatedTimeSpentInDays { get; set; }
        public int AggregatedTimeEstimateInDays { get; set; }
    }

    public class IssueType
    {
        public string Name { get; set; }
        public bool SubTask { get; set; }
    }
}
