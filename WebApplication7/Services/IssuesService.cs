using System.Text;
using WebApplication7.Models;
using WebApplication7.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using IssueAnalysisExtended.Repository.Interfaces;



namespace WebApplication7.Services
{
    public class IssuesService
    {
        private HttpClientService httpClientService;
        private IssueMapperService issueMapperService;
        private SourceService sourceService;
        private const string SOURCE_SEARCH_ENDPOINT = "rest/api/3/search";
        private const int MAX_RESULT = 100;
        private const string ISSUES_KEY = "issues";
        private const string TOTAL_KEY = "total";
        private const string RELEASES_ENDPOINT = "rest/api/3/project/{0}/versions";
        private IIssueRepository issueRepository;
        private CustomFieldsService customFieldsService;
        private ProjectService projectService;
        public IssuesService(HttpClientService httpClientService,
               SourceService sourceService,
               IssueMapperService issueMapperService,
               IIssueRepository issueRepository,
               CustomFieldsService customFieldsService,
               ProjectService projectService)
        {
            this.httpClientService = httpClientService;
            this.issueMapperService = issueMapperService;
            this.issueRepository = issueRepository;
            this.sourceService = sourceService;
            this.customFieldsService = customFieldsService;
            this.projectService = projectService;
        }

        public async Task<List<Release>> FetchReleasesFromSource()
        {
            SourceCredentials sourceCredentials = await sourceService.GetSourceCredentialsAsync();
            var project = await projectService.GetProjectDetails();

            if(project == null)
            {
                throw new Exception("Project not configured!");
            }

            string endPoint = String.Format(RELEASES_ENDPOINT, project.Key);
            Uri url = new Uri(new Uri(sourceCredentials.SourceURL), endPoint);

            HttpResponseMessage httpResponse = await httpClientService.SendGetRequestWithBasicAuthHeaders(url.ToString(), sourceCredentials);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Error code: {httpResponse.StatusCode}, Content: {await httpResponse.Content.ReadAsStringAsync()}");
            }

            string responseBody = await httpResponse.Content.ReadAsStringAsync();
            List<Release> releases = JsonConvert.DeserializeObject<List<Release>>(responseBody);
            
            return releases;
        }

        public async Task<List<Issue>> FetchIssuesAgainstRelease(string fixVersion)
        {
            List<Issue> issuesList = new List<Issue>();
            SourceCredentials sourceCredentials = await sourceService.GetSourceCredentialsAsync();
            DataClientCursor dataClientCursor = new DataClientCursor();
            string storyPointsCfValue = await customFieldsService.GetCustomFieldValueAgainstKey(CustomFieldsService.STORY_POINTS_CF_KEY);
            string teamBoardCfValue = await customFieldsService.GetCustomFieldValueAgainstKey(CustomFieldsService.TEAM_BOARD_CF_KEY);
            while (dataClientCursor.NextIterationPossible)
            {
                Uri url = new Uri(new Uri(sourceCredentials.SourceURL), SOURCE_SEARCH_ENDPOINT);
                string requestBody = await getRequestBody($"fixVersion = '{fixVersion}'", dataClientCursor.Iteration * MAX_RESULT, storyPointsCfValue, teamBoardCfValue);
                HttpResponseMessage httpResponse = await httpClientService.SendPostRequest(url.ToString(),
                                                        requestBody,
                                                        sourceCredentials);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception($"Error code: {httpResponse.StatusCode}, Content: {await httpResponse.Content.ReadAsStringAsync()}");
                }

                string responseBody = await httpResponse.Content.ReadAsStringAsync();
                JObject jsonObject = JObject.Parse(responseBody);
                
                dataClientCursor.TotalRecords = issueMapperService.castValueToGivenType<int>(jsonObject[TOTAL_KEY]);
                var issues = await issueMapperService.MapToIssueObject(jsonObject[ISSUES_KEY], sourceCredentials.SourceURL, storyPointsCfValue, teamBoardCfValue);
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

        private async Task<string> getRequestBody(string jql, int startAt, string storyPointsCfValue, string teamBoardCfValue)
        {
            var queryObject = new
            {
                jql = jql,
                maxResults = MAX_RESULT,
                startAt = startAt,
                expand = new[] { "changelog" },
                fields = await issueMapperService.getFieldsValues(storyPointsCfValue, teamBoardCfValue)
            };

            string json = JsonConvert.SerializeObject(queryObject);
            return json;
        }

        public IssueResponse processIssuesList(List<Issue> issues)
        {
            //filtering parent issues
            issues = issues = issues.Where(issue => !issue.IssueType.SubTask).ToList();
            //sorting issues on basics of resolved date
            string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            var issuesWithNoResolvedDate = issues.Where(issue => String.IsNullOrEmpty(issue.ResolvedDate) || String.IsNullOrWhiteSpace(issue.ResolvedDate) || String.IsNullOrEmpty(issue.CreatedDate) || String.IsNullOrWhiteSpace(issue.CreatedDate)).ToList();
            issues = issues.Where(issue => !String.IsNullOrEmpty(issue.ResolvedDate) && !String.IsNullOrWhiteSpace(issue.ResolvedDate))
                .OrderBy(issue => DateTime.ParseExact(issue.ResolvedDate, sysFormat, CultureInfo.InvariantCulture)).ToList();
            issues.AddRange(issuesWithNoResolvedDate);
            IssueResponse response = new IssueResponse();
            return response.processIssues(issues);
        }

        public async Task<IssueResponse> GetAllIssuesFromDatabase(string fixVersion)
        {
            List<Issue> issue = await issueRepository.GetAllIssuesAgainstFixVersion(fixVersion);
            return processIssuesList(issue);
        }


        public List<Issue> GetAllIssuesList()
        {
            return issueRepository.GetAllIssues().ToList();
        }
    }


}
