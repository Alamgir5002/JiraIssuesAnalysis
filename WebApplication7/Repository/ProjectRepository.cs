using IssueAnalysisExtended.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;

namespace WebApplication7.Repository
{
    public class ProjectRepository: IProjectRepository
    {
        private DatabaseContext databaseContext;
        public ProjectRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<Project> AddOrUpdateSourceProject(Project project)
        {
            var projectDetails = await GetProjectDetails();
            if(projectDetails != null)
            {
                databaseContext.Projects.Remove(projectDetails);
                await databaseContext.SaveChangesAsync();
            }

            await databaseContext.Projects.AddAsync(project);
            await databaseContext.SaveChangesAsync();
            return project;
        }

        public async Task<Project?> GetProjectDetails()
        {
            return await databaseContext.Projects.FirstOrDefaultAsync();
        }
    }
}
