using IssueAnalysisExtended.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;

namespace IssueAnalysisExtended.Repository
{
    public class IssueEstimatedAndSpentTimeRepository: IIssueEstimatedAndSpentTimeRepository
    {
        private DatabaseContext databaseContext;

        public IssueEstimatedAndSpentTimeRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task CreateOrReplaceIssueEstimatedAndSpentTime(Issue issue)
        {
            var existingTime = await getEstimatedAndSpentTimeAgainstIssueId(issue.Id);
            if (existingTime != null)
            {
                existingTime.AggregateTimeEstimate = issue.IssueEstimatedAndSpentTime.AggregateTimeEstimate;
                existingTime.AggregatedTimeEstimateInDays = issue.IssueEstimatedAndSpentTime.AggregatedTimeEstimateInDays;
                existingTime.AggregatedTimeSpentInDays = issue.IssueEstimatedAndSpentTime.AggregatedTimeSpentInDays;
                databaseContext.Update(existingTime);
            }
            else
            {
                await databaseContext.EstimatedAndSpentTimes.AddAsync(issue.IssueEstimatedAndSpentTime);
            }
        }

        private async Task<EstimatedAndSpentTime?> getEstimatedAndSpentTimeAgainstIssueId(string issueId)
        {
            return await this.databaseContext.EstimatedAndSpentTimes.FirstOrDefaultAsync(est => est.Id.Equals(issueId));
        }
    }
}
