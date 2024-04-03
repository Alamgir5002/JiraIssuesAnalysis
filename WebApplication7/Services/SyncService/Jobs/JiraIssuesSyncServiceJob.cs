using IssueAnalysisExtended.Repository;
using IssueAnalysisExtended.Repository.Interfaces;
using WebApplication7.Services;

namespace IssueAnalysisExtended.Services.SyncService.Jobs
{
    public class JiraIssuesSyncServiceJob : ISyncJob
    {
        private readonly IReleaseRepository releasesRespository;
        private readonly IssuesService issuesService;

        public JiraIssuesSyncServiceJob(IReleaseRepository _releasesRespository, IssuesService _issuesService) 
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
