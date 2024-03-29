using System.Text;
using WebApplication7.Models;
using WebApplication7.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;



namespace WebApplication7.Services
{
    public class IssuesService
    {
        private HttpClientService httpClientService;
        private SourceCredentialsRepository sourceCredentialsRepository;
        private IssueMapperService issueMapperService;
        private SourceService sourceService;
        private const string SOURCE_SEARCH_ENDPOINT = "rest/api/3/search";
        private const int MAX_RESULT = 100;
        private const string ISSUES_KEY = "issues";
        private const string TOTAL_KEY = "total";
        private const string RELEASES_ENDPOINT = "rest/api/3/project/{0}/versions";
        private IssueRepository issueRepository;
        public IssuesService(HttpClientService httpClientService,
               SourceCredentialsRepository sourceCredentialsRepository,
               SourceService sourceService,
               IssueMapperService issueMapperService,
               IssueRepository issueRepository)
        {
            this.sourceCredentialsRepository = sourceCredentialsRepository;
            this.httpClientService = httpClientService;
            this.issueMapperService = issueMapperService;
            this.issueRepository = issueRepository;
            this.sourceService = sourceService;
        }

        public async Task<List<Release>> FetchReleasesFromSource(string projectId)
        {
            SourceCredentials sourceCredentials = await sourceService.GetSourceCredentialsAsync();

            string endPoint = String.Format(RELEASES_ENDPOINT, projectId);
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
            List<Issue> issuesList = new List<Issue>();
            SourceCredentials sourceCredentials = await sourceService.GetSourceCredentialsAsync();

            DataClientCursor dataClientCursor = new DataClientCursor();
            while(dataClientCursor.NextIterationPossible)
            {
                Uri url = new Uri(new Uri(sourceCredentials.SourceURL), SOURCE_SEARCH_ENDPOINT);
                string requestBody = await getRequestBody($"fixVersion = {fixVersion}", dataClientCursor.Iteration);
                HttpResponseMessage httpResponse = await httpClientService.SendPostRequest(url.ToString(),
                                                        requestBody,
                                                        GetBasicAuthHeaders(sourceCredentials));

                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception($"Error code: {httpResponse.StatusCode}, Content: {await httpResponse.Content.ReadAsStringAsync()}");
                }

                string responseBody = await httpResponse.Content.ReadAsStringAsync();
                JObject jsonObject = JObject.Parse(responseBody);
                
                dataClientCursor.TotalRecords = issueMapperService.castValueToGivenType<int>(jsonObject[TOTAL_KEY]);

                var issues = await issueMapperService.MapToIssueObject(jsonObject[ISSUES_KEY], sourceCredentials.SourceURL);
                issues = issues.GroupBy(i => i.Id).Select(i=>i.First()).ToList();

                foreach(Issue issue in issues)
                {
                    await issueRepository.AddOrUpdateIssue(issue);
                }

                issuesList.AddRange(issues);
                dataClientCursor.Iteration += 1;

                dataClientCursor.NextIterationPossible = (dataClientCursor.Iteration * MAX_RESULT) < dataClientCursor.TotalRecords;
            }

            return issuesList;
        }

        private async Task<string> getRequestBody(string jql, int startAt)
        {
            var queryObject = new
            {
                jql = jql,
                maxResults = MAX_RESULT,
                startAt = startAt,
                expand = new[] { "changelog" },
                fields = await issueMapperService.getFieldsValues()
            };

            string json = JsonConvert.SerializeObject(queryObject);
            return json;
        }


        public List<Issue> processIssuesList(List<Issue> issues)
        {
            //filtering parent issues
            //issues = issues = issues.Where(issue => !issue.IssueType.SubTask).ToList();
            //sorting issues on basics of resolved date
            var issuesWithNoResolvedDate = issues.Where(issue => String.IsNullOrEmpty(issue.ResolvedDate) || String.IsNullOrWhiteSpace(issue.ResolvedDate)).ToList();
            issues = issues.Where(issue => !String.IsNullOrEmpty(issue.ResolvedDate) && !String.IsNullOrWhiteSpace(issue.ResolvedDate))
                .OrderBy(issue => DateTime.ParseExact(issue.ResolvedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
            issues.AddRange(issuesWithNoResolvedDate);
            IssueResponse response = new IssueResponse();
            response.processIssues(issues);
            return issues;
        }

       
    }


}
