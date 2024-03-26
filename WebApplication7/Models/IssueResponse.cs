namespace WebApplication7.Models
{
    public class IssueResponse
    {
        public List<Issue> Issues { get; set; }
        public int TotalIssues { get; set; }
        public int NumberOfInProgressIssues { get; set; }
        public int NumberOfResolvedIssues { get; set; }
        public int TotalStoryPoints { get; set; }


        public IssueResponse processIssues(List<Issue> issues)
        {
            IssueResponse response = new IssueResponse();
            response.Issues = issues;
            response.TotalIssues = issues.Count;
            response.TotalStoryPoints = issues.Sum(issue => issue.StoryPoints);
            response.NumberOfInProgressIssues = issues.Where(issue=> String.IsNullOrEmpty(issue.ResolvedDate) || String.IsNullOrWhiteSpace(issue.ResolvedDate)).ToList().Count();   
            response.NumberOfResolvedIssues = issues.Count - response.NumberOfInProgressIssues;
            return response;
        }
    }
}
