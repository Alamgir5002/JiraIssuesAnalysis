using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;

namespace WebApplication7.Repository
{
    public class IssueRepository
    {
        private DatabaseContext databaseContext;
        public IssueRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public Issue AddOrUpdateIssue(Issue issue)
        {
            Issue? existingIssue = getIssueById(issue.Id);
            if (existingIssue != null)
            {
                databaseContext.Entry(existingIssue).State = EntityState.Detached;
                databaseContext.Update(issue);
            }
            else
            {
                databaseContext.Issues.Add(issue);
            }

            var existingTime = GetEstimatedAndSpentTimeAgainstIssueId(issue.Id);
            if (existingTime != null)
            {
                existingTime.AggregatedTimeSpent = issue.IssueEstimatedAndSpentTime.AggregatedTimeSpent;
                existingTime.AggregatedTimeSpentInDays = issue.IssueEstimatedAndSpentTime.AggregatedTimeSpentInDays;
                existingTime.AggregateTimeEstimate = issue.IssueEstimatedAndSpentTime.AggregateTimeEstimate;
                existingTime.AggregatedTimeEstimateInDays = issue.IssueEstimatedAndSpentTime.AggregatedTimeEstimateInDays;
                databaseContext.Update(existingTime);
            }
            else
            {
                databaseContext.EstimatedAndSpentTimes.Add(issue.IssueEstimatedAndSpentTime);
            }

            databaseContext.SaveChanges();
            return issue;
        }

        public EstimatedAndSpentTime AddEstimateAndSpentTime(EstimatedAndSpentTime estimatedAndSpentTime)
        {
            databaseContext.EstimatedAndSpentTimes.Add(estimatedAndSpentTime);
            databaseContext.SaveChanges();
            return estimatedAndSpentTime;
        }

        public Issue? getIssueById(string issueId)
        {
            return this.databaseContext.Issues.FirstOrDefault(issue => issue.Id.Equals(issueId));
        }

        public EstimatedAndSpentTime? GetEstimatedAndSpentTimeAgainstIssueId(string issueId)
        {
            return this.databaseContext.EstimatedAndSpentTimes.FirstOrDefault(est => est.IssueId.Equals(issueId));
        }

        public IEnumerable<Issue> GetAllIssues()
        {
            return databaseContext.Issues.Include(issue => issue.IssueEstimatedAndSpentTime).ToList();
        }
    }
}
