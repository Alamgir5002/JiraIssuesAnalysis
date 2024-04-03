using IssueAnalysisExtended.Repository;
using WebApplication7.Services;

namespace IssueAnalysisExtended.Services.SyncService.Jobs
{
    public class JiraIssuesSyncServiceJob : ISyncJob
    {
        private readonly ReleasesRespository releasesRepository;
        private readonly IssuesService issuesService;

        public JiraIssuesSyncServiceJob(ReleasesRespository _releasesRespository, IssuesService _issuesService)
        {
            releasesRepository = _releasesRespository;
            issuesService = _issuesService;
        }

        public async Task Execute()
        {
            var existingReleases = await releasesRepository.GetAllExistingReleasesAsync();
            foreach (var existingRelease in existingReleases)
            {
                await issuesService.FetchIssuesAgainstRelease(existingRelease.Name);
            }
        }

    }
}
