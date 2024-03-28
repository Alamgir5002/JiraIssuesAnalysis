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

        public async Task<Issue> AddOrUpdateIssue(Issue issue)
        {
            issue.FixVersions = await AddReleases(issue);

            await CreateOrUpdateParent(issue);

            await CreateOrReplaceTeamBoard(issue);

            await CreateOrReplaceIssueType(issue);

            Issue? existingIssue = await getIssueById(issue.Id);
            if (existingIssue != null)
            {
                databaseContext.Entry(existingIssue).State = EntityState.Detached;
                databaseContext.Update(issue);
            }
            else
            {
                await databaseContext.Issues.AddAsync(issue);
            }

            await CreateOrReplaceIssueEstimatedAndSpentTime(issue);

            await databaseContext.SaveChangesAsync();
            return issue;
        }

        public EstimatedAndSpentTime AddEstimateAndSpentTime(EstimatedAndSpentTime estimatedAndSpentTime)
        {
            databaseContext.EstimatedAndSpentTimes.Add(estimatedAndSpentTime);
            databaseContext.SaveChanges();
            return estimatedAndSpentTime;
        }

        public async Task<Issue?> getIssueById(string issueId)
        {
            return await this.databaseContext.Issues.FirstOrDefaultAsync(issue => issue.Id.Equals(issueId));
        }

        public async Task<EstimatedAndSpentTime?> GetEstimatedAndSpentTimeAgainstIssueId(string issueId)
        {
            return await this.databaseContext.EstimatedAndSpentTimes.FirstOrDefaultAsync(est => est.IssueId.Equals(issueId));
        }

        public IEnumerable<Issue> GetAllIssues()
        {
            return databaseContext.Issues
                .Include(issue => issue.IssueEstimatedAndSpentTime)
                .Include(issue => issue.IssueType)
                .Include(issue => issue.Parent)
                .Include(issue => issue.TeamBoard).ToList();
        }

        public async Task<IssueType?> GetIssueTypeByIssueTypeId(string issueTypeId) {
            return await databaseContext.IssueTypes.FirstOrDefaultAsync(it => it.Id.Equals(issueTypeId));
        }

        public async Task<Parent?> GetParentByParentId(string parentId) {
            return await databaseContext.ParentIssues.FirstOrDefaultAsync(parent => parent.Id.Equals(parentId));
        }

        public async Task<TeamBoard?> GetTeamBoardById(string teamBoardId)
        {
            return await databaseContext.TeamBoards.FirstOrDefaultAsync(teamBoard => teamBoard.Id.Equals(teamBoardId));    
        }

        public async Task<List<IssueRelease>> AddReleases(Issue issue)
        {
            List<IssueRelease> issueReleases = new List<IssueRelease>();

            foreach(var release in issue.FixVersions)
            {
                var ir = new IssueRelease();
                var existingRelease = await GetReleaseById(release.Release.Id);

                if (existingRelease != null)
                {
                    ir.ReleaseId = existingRelease.Id;
                    ir.Release = existingRelease;  
                }
                else
                {
                    await databaseContext.Releases.AddAsync(release.Release);
                    await databaseContext.SaveChangesAsync();
                    ir.ReleaseId = release.ReleaseId;
                    ir.Release = release.Release;
                }

                ir.Issue = issue;
                ir.IssueId = issue.Id;
                issueReleases.Add(ir);
            }
            
            return issueReleases;
        }

        public async Task<Release?> GetReleaseById(string releaseId)
        {
            return await databaseContext.Releases.FirstOrDefaultAsync(release => release.Id.Equals(releaseId));
        }

        public async Task CreateOrUpdateParent(Issue issue)
        {
            if(issue.Parent != null)
            {
                var parentIssue = await GetParentByParentId(issue.Parent.Id);
                if (parentIssue != null)
                {
                    issue.Parent = parentIssue;
                    issue.ParentId = parentIssue.ParentId;
                }
                else
                {
                    await databaseContext.ParentIssues.AddAsync(issue.Parent);
                    await databaseContext.SaveChangesAsync();
                }
            }
        }

        public async Task CreateOrReplaceTeamBoard(Issue issue)
        {
            if (issue.TeamBoard != null)
            {
                var existingTeamBoard = await GetTeamBoardById(issue.TeamBoard.Id);
                if (existingTeamBoard != null)
                {
                    issue.TeamBoard = existingTeamBoard;
                    issue.ParentId = existingTeamBoard.TeamBoardId;
                }
                else
                {
                    await databaseContext.TeamBoards.AddAsync(issue.TeamBoard);
                    await databaseContext.SaveChangesAsync();
                }
            }
        }

        public async Task CreateOrReplaceIssueType(Issue issue)
        {
            var issueType = await GetIssueTypeByIssueTypeId(issue.IssueType.Id);
            if (issueType == null)
            {
                await databaseContext.IssueTypes.AddAsync(issue.IssueType);
                await databaseContext.SaveChangesAsync();
            }
            else
            {
                issue.IssueType = issueType;
                issue.IssueTypeId = issueType.IssueTypeId;
            }
        }

        public async Task CreateOrReplaceIssueEstimatedAndSpentTime(Issue issue)
        {
            var existingTime = await GetEstimatedAndSpentTimeAgainstIssueId(issue.Id);
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
                await databaseContext.EstimatedAndSpentTimes.AddAsync(issue.IssueEstimatedAndSpentTime);
            }
        }
    }
}
