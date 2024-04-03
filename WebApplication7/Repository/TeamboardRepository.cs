using IssueAnalysisExtended.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;

namespace IssueAnalysisExtended.Repository
{
    public class TeamboardRepository: ITeamboardRepository
    {
        private DatabaseContext databaseContext;
        public TeamboardRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task CreateOrUpdateTeamBoard(Issue issue)
        {
            if (issue.TeamBoard != null)
            {
                var existingTeamBoard = await getTeamBoardById(issue.TeamBoard.Id);
                if (existingTeamBoard != null)
                {
                    issue.TeamBoard = existingTeamBoard;
                    issue.TeamBoardId = existingTeamBoard.Id;
                }
                else
                {
                    await databaseContext.TeamBoards.AddAsync(issue.TeamBoard);
                }
                await databaseContext.SaveChangesAsync();
            }
        }
        private async Task<TeamBoard?> getTeamBoardById(string teamBoardId)
        {
            return await databaseContext.TeamBoards.FirstOrDefaultAsync(teamBoard => teamBoard.Id.Equals(teamBoardId));
        }

    }
}
