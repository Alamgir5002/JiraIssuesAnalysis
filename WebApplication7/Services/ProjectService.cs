using IssueAnalysisExtended.Models;
using IssueAnalysisExtended.Repository.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using WebApplication7.Models;
using WebApplication7.Repository;

namespace WebApplication7.Services
{
    public class ProjectService
    {
        private const string SOURCE_PROJECTS_ENDPOINT = "rest/api/3/project";
        private HttpClientService httpClientService;
        private IProjectRepository projectRepository;

        public ProjectService(
            HttpClientService httpClientService, 
            IProjectRepository projectRepository)
        {
            this.httpClientService = httpClientService;
            this.projectRepository = projectRepository;
        }

        public async Task<List<Project>> FetchAllProjectsFromSource(SourceCredentials sourceCredentials)
        {
            Uri url = new Uri(new Uri(sourceCredentials.SourceURL), SOURCE_PROJECTS_ENDPOINT);
            HttpResponseMessage httpResponse = await httpClientService.SendGetRequestWithBasicAuthHeaders(url.ToString(),
                                                        sourceCredentials);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Error code: {httpResponse.StatusCode}, Content: {await httpResponse.Content.ReadAsStringAsync()}");
            }

            string responseBody = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Project>>(responseBody);
        }

        public async Task<Project> AddOrUpdateSourceProject(Project project)
        {
            var resp = await projectRepository.AddOrUpdateSourceProject(project);
            return resp;
        }

        public async Task<Project?> GetProjectDetails()
        {
            return await projectRepository.GetProjectDetails();
        }


    }
}

