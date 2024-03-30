using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;

namespace IssueAnalysisExtended.Repository
{
    public class ReleasesRespository
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
    }
}
