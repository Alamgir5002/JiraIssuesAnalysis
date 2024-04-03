using IssueAnalysisExtended.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;

namespace WebApplication7.Repository
{
    public class IssueRepository: IIssueRepository
    {
        private DatabaseContext databaseContext;
        private IReleaseRepository releaseRepository;
        private IParentRepository parentRepository;
        private ITeamboardRepository teamboardRepository;
        private IIssueTypeRepository issueTypeRepository;
        private IIssueEstimatedAndSpentTimeRepository issueEstimatedAndSpentTimeRepository;
        public IssueRepository(DatabaseContext databaseContext,
            IReleaseRepository releaseRepository,
            IParentRepository parentRepository,
            ITeamboardRepository teamboardRepository,
            IIssueTypeRepository issueTypeRepository,
            IIssueEstimatedAndSpentTimeRepository issueEstimatedAndSpentTimeRepository)
        {
            this.databaseContext = databaseContext;
            this.releaseRepository = releaseRepository;
            this.parentRepository = parentRepository;
            this.teamboardRepository = teamboardRepository;
            this.issueTypeRepository = issueTypeRepository;
            this.issueEstimatedAndSpentTimeRepository = issueEstimatedAndSpentTimeRepository;
        }

        public async Task<Issue> AddOrUpdateIssue(Issue issue)
        {
            issue.FixVersions = await releaseRepository.AddOrUpdateIssueReleases(issue);

            await parentRepository.CreateOrUpdateParent(issue); 

            await teamboardRepository.CreateOrUpdateTeamBoard(issue);

            await issueTypeRepository.CreateOrUpdateIssueType(issue);

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

            await issueEstimatedAndSpentTimeRepository.CreateOrReplaceIssueEstimatedAndSpentTime(issue);

            await databaseContext.SaveChangesAsync();
            return issue;
        }



        private async Task<Issue?> getIssueById(string issueId)
        {
            return await this.databaseContext.Issues.FirstOrDefaultAsync(issue => issue.Id.Equals(issueId));
        }


        public IEnumerable<Issue> GetAllIssues()
        {
            return databaseContext.Issues
                .Include(issue => issue.IssueEstimatedAndSpentTime)
                .Include(issue => issue.IssueType)
                .Include(issue => issue.Parent)
                .Include(issue => issue.TeamBoard)
                .Include(issue => issue.FixVersions).ThenInclude(ir => ir.Release);
        }

        public async Task<List<Issue>> GetAllIssuesAgainstFixVersion(string fixVersion)
        {
            var issues = await databaseContext.Issues.
                Where(issue => issue.FixVersions.
                    Any(ir => ir.Release.Name.Equals(fixVersion)))
                .Include(issue => issue.IssueEstimatedAndSpentTime)
                .Include(issue => issue.IssueType)
                .Include(issue => issue.Parent)
                .Include(issue => issue.TeamBoard)
                .Include(issue => issue.FixVersions)
                .ThenInclude(ir => ir.Release)
                .ToListAsync();
            return issues;
        }
    }
}
