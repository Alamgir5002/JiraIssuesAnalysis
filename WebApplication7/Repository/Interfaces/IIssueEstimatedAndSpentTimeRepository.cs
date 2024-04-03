using WebApplication7.Models;

namespace IssueAnalysisExtended.Repository.Interfaces
{
    public interface IIssueEstimatedAndSpentTimeRepository
    {
        public Task CreateOrReplaceIssueEstimatedAndSpentTime(Issue issue);
    }
}
