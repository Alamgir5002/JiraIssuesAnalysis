using IssueAnalysisExtended.Models;
using System.Text;
using WebApplication7.Models;
using WebApplication7.Repository;

namespace WebApplication7.Services
{

    public class SourceService
    {
        private const string JIRA_INSTANCE_INFO_URL = "/rest/api/3/serverInfo";
        private const string JIRA_USER_CREDENTIALS_INFO_URL = "/rest/api/3/myself";
        private HttpClientService httpClientService;
        private SourceCredentialsRepository sourceCredentialsRepository;
        private ProjectService projectService;
        private CustomFieldsService customFieldsService;
        public SourceService(HttpClientService httpClientService, 
            SourceCredentialsRepository sourceCredentialsRepository,
            ProjectService projectService,
            CustomFieldsService customFieldsService)
        {
            this.httpClientService = httpClientService;
            this.sourceCredentialsRepository = sourceCredentialsRepository;
            this.projectService = projectService;
            this.customFieldsService = customFieldsService;
        }

        public async Task<SourceCredentials> ValidateAndSaveCredentials(SourceCredentials sourceCredentials)
        {
            await validateSourceCredentials(sourceCredentials);
            return await sourceCredentialsRepository.AddSourceCredentials(sourceCredentials);
        }

        private async Task validateSourceCredentials(SourceCredentials sourceCredentials)
        {
            await validateSourceUrl(sourceCredentials.SourceURL);
            await validateSourceDetails(sourceCredentials);
        }

        private async Task validateSourceUrl(string sourceUrl)
        {
            string url = sourceUrl + JIRA_INSTANCE_INFO_URL;
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new Exception($"Invalid URL : {sourceUrl}");
            }

            HttpResponseMessage httpResponse = await httpClientService.SendGetRequest(url);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception($"JIRA instance not found at URL : {sourceUrl}");
            }
        }

        private async Task validateSourceDetails(SourceCredentials sourceCredentials)
        {
            string url = sourceCredentials.SourceURL + JIRA_USER_CREDENTIALS_INFO_URL;

            HttpResponseMessage httpResponse = await httpClientService.SendGetRequestWithBasicAuthHeaders(url, sourceCredentials);
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Invalid source token or user email");
            }
        }

        public async Task<SourceCredentials> GetSourceCredentialsAsync()
        {
            var sourceCredentials = await sourceCredentialsRepository.GetSourceCredentialsAsync();
            if(sourceCredentials == null)
            {
                throw new Exception("Source details not found");
            }

            return sourceCredentials;
        }

        public async Task<SourceFieldsResponse> GetSourceFields()
        {
            var sourceCredentials = await GetSourceCredentialsAsync();
            var sourceCustomFields = await customFieldsService.GetAllCustomFieldsFromSource(sourceCredentials);
            var userSelectedCustomFields = await customFieldsService.GetAllCustomFieldsAsync();
            var sourceProjects = await projectService.FetchAllProjectsFromSource(sourceCredentials);
            var userSelectedProject = await projectService.GetProjectDetails();

            return SourceFieldsResponse.processSourceResponse(sourceCustomFields, userSelectedCustomFields, sourceProjects, userSelectedProject);
        }
    }
}
