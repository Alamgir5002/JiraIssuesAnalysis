using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using WebApplication7.Models;
using WebApplication7.Repository;

namespace WebApplication7.Services
{
    public class ProjectService
    {
        private SourceService sourceService;
        private const string SOURCE_PROJECTS_ENDPOINT = "rest/api/3/project";
        private HttpClientService httpClientService;
        private ProjectRepository projectRepository;

        public ProjectService(SourceService sourceService, HttpClientService httpClientService, ProjectRepository projectRepository)
        {
            this.sourceService = sourceService;
            this.httpClientService = httpClientService;
            this.projectRepository = projectRepository;
        }

        public async Task<List<Project>> FetchAllProjectsFromSource()
        {
            var sourceDetails = await sourceService.GetSourceCredentialsAsync();
            Uri url = new Uri(new Uri(sourceDetails.SourceURL), SOURCE_PROJECTS_ENDPOINT);
            HttpResponseMessage httpResponse = await httpClientService.SendGetRequestWithBasicAuthHeaders(url.ToString(),
                                                        sourceDetails);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Error code: {httpResponse.StatusCode}, Content: {await httpResponse.Content.ReadAsStringAsync()}");
            }

            string responseBody = await httpResponse.Content.ReadAsStringAsync();
            try
            {
                return JsonConvert.DeserializeObject<List<Project>>(responseBody);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return null;
        }

        public async Task<Project> AddSourceProject(Project project)
        {
            var resp = await projectRepository.AddSourceProject(project);
            return resp;
        }


    }
}

