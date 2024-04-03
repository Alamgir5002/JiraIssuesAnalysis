using WebApplication7.Models;

namespace IssueAnalysisExtended.Repository.Interfaces
{
    public interface IParentRepository
    {
        public Task CreateOrUpdateParent(Issue issue);
    }
}
