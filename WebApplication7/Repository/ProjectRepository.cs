using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;

namespace WebApplication7.Repository
{
    public class ProjectRepository
    {
        private DatabaseContext databaseContext;
        public ProjectRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<Project> AddOrUpdateSourceProject(Project project)
        {
            var projectDetails = await GetProjectDetails();
            if(projectDetails == null)
            {
                await databaseContext.Projects.AddAsync(project);
            }
            else
            {
                projectDetails.Name = project.Name;
                projectDetails.Id = project.Id;
                projectDetails.Key = project.Key;
                databaseContext.Entry(projectDetails).State = EntityState.Modified;
            }

            await databaseContext.SaveChangesAsync();
            return project;
        }

        public async Task<Project?> GetProjectDetails()
        {
            return await databaseContext.Projects.FirstOrDefaultAsync();
        }
    }
}
