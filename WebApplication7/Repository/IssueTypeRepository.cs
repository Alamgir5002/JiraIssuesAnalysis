using IssueAnalysisExtended.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;

namespace IssueAnalysisExtended.Repository
{
    public class IssueTypeRepository: IIssueTypeRepository
    {
        private DatabaseContext databaseContext;

        public IssueTypeRepository(DatabaseContext databaseContext) {
            this.databaseContext = databaseContext;
        }

        public async Task CreateOrUpdateIssueType(Issue issue)
        {
            var issueType = await getIssueTypeByIssueTypeId(issue.IssueType.Id);
            if (issueType == null)
            {
                await databaseContext.IssueTypes.AddAsync(issue.IssueType);
            }
            else
            {
                issue.IssueType = issueType;
                issue.IssueTypeId = issueType.Id;
            }
            await databaseContext.SaveChangesAsync();
        }

        private async Task<IssueType?> getIssueTypeByIssueTypeId(string issueTypeId)
        {
            return await databaseContext.IssueTypes.FirstOrDefaultAsync(it => it.Id.Equals(issueTypeId));
        }

    }
}
