using WebApplication7.Models;

namespace IssueAnalysisExtended.Repository.Interfaces
{
    public interface IReleaseRepository
    {
        public Task<List<Release>> GetAllExistingReleasesAsync();
        public Task AddOrUpdateReleaseAsync(Release release);

        public Task<List<IssueRelease>> AddOrUpdateIssueReleases(Issue issue);

    }
}
