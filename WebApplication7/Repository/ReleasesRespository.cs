using IssueAnalysisExtended.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;

namespace IssueAnalysisExtended.Repository
{
    public class ReleasesRespository: IReleaseRepository
    {
        private readonly DatabaseContext databaseContext;
        public ReleasesRespository(DatabaseContext _databaseContext)
        {
            databaseContext = _databaseContext;
        }

        public async Task AddOrUpdateReleaseAsync(Release release)
        {
            await databaseContext.Releases.AddAsync(release);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<List<Release>> GetAllExistingReleasesAsync()
        {
            return await databaseContext.Releases.ToListAsync();
        }

        public async Task<List<IssueRelease>> AddOrUpdateIssueReleases(Issue issue)
        {
            List<IssueRelease> issueReleases = new List<IssueRelease>();

            foreach (var release in issue.FixVersions)
            {
                var ir = new IssueRelease();
                var existingRelease = await getReleaseById(release.Release.Id);

                if (existingRelease != null)
                {
                    ir.ReleaseId = existingRelease.Id;
                    ir.Release = existingRelease;
                }
                else
                {
                    await databaseContext.Releases.AddAsync(release.Release);
                    ir.ReleaseId = release.ReleaseId;
                    ir.Release = release.Release;
                }

                await databaseContext.SaveChangesAsync();

                ir.Issue = issue;
                ir.IssueId = issue.Id;
                issueReleases.Add(ir);
            }

            return issueReleases;
        }

        private async Task<Release?> getReleaseById(string releaseId)
        {
            return await databaseContext.Releases.FirstOrDefaultAsync(release => release.Id.Equals(releaseId));
        }

    }
}
