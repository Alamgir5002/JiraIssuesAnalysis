using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;

namespace WebApplication7.Repository
{
    public class SourceCredentialsRepository
    {
        private DatabaseContext databaseContext;
        public SourceCredentialsRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<SourceCredentials> AddSourceCredentials(SourceCredentials sourceCredentials)
        {
            await databaseContext.SourceCredentials.AddAsync(sourceCredentials);
            await databaseContext.SaveChangesAsync();
            return sourceCredentials;
        }

        public async Task<SourceCredentials?> GetSourceCredentialsAsync()
        {
            SourceCredentials sourceCredentials = await databaseContext.SourceCredentials.FirstOrDefaultAsync();
            return sourceCredentials;
        }
    }
}
