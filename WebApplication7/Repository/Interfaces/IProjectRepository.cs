using WebApplication7.Models;

namespace IssueAnalysisExtended.Repository.Interfaces
{
    public interface IProjectRepository
    {
        public Task<Project?> GetProjectDetails();
        public Task<Project> AddOrUpdateSourceProject(Project project);
    }
}
