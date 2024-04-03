
namespace IssueAnalysisExtended.Services.SyncService.Jobs
{
    public class BootstrapJob : ISyncJob
    {
        public void Execute(List<string> releases)
        {
            foreach (var release in releases) { }
        }

        public Task Execute()
        {
            throw new NotImplementedException();
        }
    }
}
