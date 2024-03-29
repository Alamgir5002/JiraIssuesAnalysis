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

        public async Task<Project> AddSourceProject(Project project)
        {
            await databaseContext.Projects.AddAsync(project);
            await databaseContext.SaveChangesAsync();
            return project;
        }
    }
}
