using IssueAnalysisExtended.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;

namespace IssueAnalysisExtended.Repository
{
    public class ParentRepository: IParentRepository
    {
        private DatabaseContext databaseContext;
        public ParentRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task CreateOrUpdateParent(Issue issue)
        {
            if (issue.Parent != null)
            {
                var parentIssue = await getParentByParentId(issue.Parent.Id);
                if (parentIssue != null)
                {
                    issue.Parent = parentIssue;
                    issue.ParentId = parentIssue.Id;
                }
                else
                {
                    await databaseContext.ParentIssues.AddAsync(issue.Parent);
                }
                await databaseContext.SaveChangesAsync();
            }
        }

        private async Task<Parent?> getParentByParentId(string parentId)
        {
            return await databaseContext.ParentIssues.FirstOrDefaultAsync(parent => parent.Id.Equals(parentId));
        }
    }
}
