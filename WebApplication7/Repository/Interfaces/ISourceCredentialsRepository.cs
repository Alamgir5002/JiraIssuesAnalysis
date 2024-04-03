using WebApplication7.Models;

namespace IssueAnalysisExtended.Repository.Interfaces
{
    public interface ISourceCredentialsRepository
    {
        public Task<SourceCredentials?> GetSourceCredentialsAsync();

        public Task<SourceCredentials> AddSourceCredentials(SourceCredentials sourceCredentials);

    }
}
