using WebApplication7.Models;

namespace IssueAnalysisExtended.Repository.Interfaces
{
    public interface ITeamboardRepository
    {
        public Task CreateOrUpdateTeamBoard(Issue issue);
    }
}
