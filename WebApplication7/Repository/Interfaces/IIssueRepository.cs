using WebApplication7.Models;

namespace IssueAnalysisExtended.Repository.Interfaces
{
    public interface IIssueRepository
    {
        public Task<Issue> AddOrUpdateIssue(Issue issue);
        public Task<List<Issue>> GetAllIssuesAgainstFixVersion(string fixVersion);
        public IEnumerable<Issue> GetAllIssues();
    }
}
