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
            if(issue.Parent!= null)
            {
                var parentIssue = GetParentByParentId(issue.Parent.Id);
                if(parentIssue != null)
                {
                    issue.Parent = parentIssue;
                    issue.ParentId = parentIssue.ParentId;
                }
                else
                {
                    databaseContext.ParentIssues.Add(issue.Parent);
                    databaseContext.SaveChanges();  
                }
            }

            if(issue.TeamBoard!= null) { 
                var existingTeamBoard = GetTeamBoardById(issue.TeamBoard.Id);   
                if(existingTeamBoard != null)
                {
                    issue.TeamBoard = existingTeamBoard;
                    issue.ParentId = existingTeamBoard.TeamBoardId;
                }
                else
                {
                    databaseContext.TeamBoards.Add(issue.TeamBoard);
                    databaseContext.SaveChanges();  
                }
            }

            var issueType = GetIssueTypeByIssueTypeId(issue.IssueType.Id);
            if (issueType == null)
            {
                databaseContext.IssueTypes.Add(issue.IssueType);
                databaseContext.SaveChanges();
            }
            else
            {
                issue.IssueType = issueType;
                issue.IssueTypeId = issueType.IssueTypeId;
            }

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
            return databaseContext.Issues
                .Include(issue => issue.IssueEstimatedAndSpentTime)
                .Include(issue => issue.IssueType)
                .Include(issue => issue.Parent)
                .Include(issue => issue.TeamBoard).ToList();
        }

        public IssueType? GetIssueTypeByIssueTypeId(string issueTypeId) {
            return databaseContext.IssueTypes.FirstOrDefault(it => it.Id.Equals(issueTypeId));
        }

        public Parent? GetParentByParentId(string parentId) {
            return databaseContext.ParentIssues.FirstOrDefault(parent => parent.Id.Equals(parentId));
        }

        public TeamBoard? GetTeamBoardById(string teamBoardId)
        {
            return databaseContext.TeamBoards.FirstOrDefault(teamBoard => teamBoard.Id.Equals(teamBoardId));    
        }
    }
}
