using IssueAnalysisExtended.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;

namespace IssueAnalysisExtended.Repository
{
    public class SyncedReleasesRespository
    {
        private readonly DatabaseContext databaseContext;

        public SyncedReleasesRespository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<List<SyncedRelease>> GetAllSynchedReleasesAsync()
        {
            return await databaseContext.syncedReleases.ToListAsync();
        }

        public async Task AddSyncedReleaseAsync(SyncedRelease syncedRelease)
        {
            await databaseContext.syncedReleases.AddAsync(syncedRelease);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<SyncedRelease?> GetRelease(string fixVersion)
        {
            return await databaseContext.syncedReleases.FirstOrDefaultAsync(rel => rel.Name.Equals(fixVersion));
        }
    }
}
