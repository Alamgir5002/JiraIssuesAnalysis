using IssueAnalysisExtended.Repository;
using WebApplication7.Services;

namespace IssueAnalysisExtended.Services.SyncService
{
    public class JiraIssuesSyncServiceJob
    {
        private readonly ReleasesRespository releasesRespository;
        private readonly IssuesService issuesService;

        public JiraIssuesSyncServiceJob(ReleasesRespository _releasesRespository, IssuesService _issuesService) 
        {
            releasesRespository = _releasesRespository;
            issuesService = _issuesService;
        }

        public async Task Execute()
        {
            var existingReleases = await releasesRespository.GetAllExistingReleasesAsync();
            foreach(var existingRelease in existingReleases)
            {
                await issuesService.FetchIssuesAgainstRelease(existingRelease.Name);
            }
        }

    }
}
