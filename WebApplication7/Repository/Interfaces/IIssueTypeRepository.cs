using WebApplication7.Models;

namespace IssueAnalysisExtended.Repository.Interfaces
{
    public interface IIssueTypeRepository
    {
        public Task CreateOrUpdateIssueType(Issue issue);
    }
}
