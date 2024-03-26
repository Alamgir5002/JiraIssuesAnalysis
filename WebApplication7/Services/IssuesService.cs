using System;
using System.Text;
using System.Text.Json.Nodes;
using WebApplication7.Models;
using WebApplication7.Repository;
using Newtonsoft.Json;
using NJsonSchema;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;


namespace WebApplication7.Services
{
    public class IssuesService
    {
        private HttpClientService httpClientService;
        private SourceCredentialsRepository sourceCredentialsRepository;
        private IssueMapperService issueMapperService;
        private const string SOURCE_SEARCH_ENDPOINT = "rest/api/3/search";
        public IssuesService(HttpClientService httpClientService, 
            SourceCredentialsRepository sourceCredentialsRepository,
            IssueMapperService issueMapperService)
        {
            this.sourceCredentialsRepository = sourceCredentialsRepository;
            this.httpClientService = httpClientService;
            this.issueMapperService = issueMapperService;
        }

        public async Task<List<Release>> FetchReleasesFromSource(string projectId)
        {
            SourceCredentials sourceCredentials = await sourceCredentialsRepository.GetSourceCredentialsAsync();
            if (sourceCredentials == null)
            {
                throw new Exception($"Source details not found");
            }

            string endPoint = $"rest/api/3/project/{projectId}/versions";
            Uri url = new Uri(new Uri(sourceCredentials.SourceURL), endPoint);
            HttpResponseMessage httpResponse = await httpClientService.SendGetRequestWithBasicAuthHeaders(url.ToString(), GetBasicAuthHeaders(sourceCredentials));
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Error code: {httpResponse.StatusCode}, Content: {await httpResponse.Content.ReadAsStringAsync()}");
            }
            string responseBody = await httpResponse.Content.ReadAsStringAsync();
            List<Release> releases = JsonConvert.DeserializeObject<List<Release>>(responseBody);
            return releases;
        }

        public string GetBasicAuthHeaders(SourceCredentials sourceCredentials)
        {
            string basicAuthString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{sourceCredentials.SourceUserEmail}:{sourceCredentials.SourceAuthToken}"));
            return basicAuthString;
        }

        public async Task<List<Issue>> FetchIssuesAgainstRelease(string fixVersion)
        {
            SourceCredentials sourceCredentials = await sourceCredentialsRepository.GetSourceCredentialsAsync();
            if (sourceCredentials == null)
            {
                throw new Exception($"Source details not found");
            }

            Uri url = new Uri(new Uri(sourceCredentials.SourceURL), SOURCE_SEARCH_ENDPOINT);
            HttpResponseMessage httpResponse = await httpClientService.SendPostRequest(url.ToString(), 
                                                    getRequestBody($"fixVersion = {fixVersion}"),
                                                    GetBasicAuthHeaders(sourceCredentials));

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Error code: {httpResponse.StatusCode}, Content: {await httpResponse.Content.ReadAsStringAsync()}");
            }
            string responseBody = await httpResponse.Content.ReadAsStringAsync();
            var issues = issueMapperService.MapToIssueObject(responseBody, sourceCredentials.SourceURL);
            return issues;
        }

        private string getRequestBody(string jql)
        {
            var queryObject = new
            {
                jql = jql,
                maxResults = 50,
                startAt = 0,
                expand = new[] { "changelog" },
                fields = getFieldsValues()
            };

            string json = JsonConvert.SerializeObject(queryObject);
            return json;
        }

        private List<string> getFieldsValues()
        {
            var fields = new List<string>{
                "issuetype",
                "parent",
                "timespent",
                "timeoriginalestimate",
                "timeestimate",
                "resolution",
                "aggregatetimeestimate",
                "aggregatetimeoriginalestimate",
                "aggregatetimespent",
                "created",
                "description",
                "priority",
                "resolutiondate",
                "status",
                "subtasks",
                "summary",
                "timeoriginalestimate",
                "fixVersions",
                "customfield_10021",
                "customfield_10024"
            };

            return fields;
        }
    }


}
