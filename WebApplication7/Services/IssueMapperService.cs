using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApplication7.Models;

namespace WebApplication7.Services
{
    public class IssueMapperService
    {
        private CustomFieldsService customFieldsService;
        public IssueMapperService(CustomFieldsService customFieldsService)
        {
            this.customFieldsService = customFieldsService;
        }

        public async Task<List<Issue>> MapToIssueObject(JToken issuesObject, string sourceUrl)
        {
            // Get custom field values
            string storyPointsCfValue = await customFieldsService.GetCustomFieldValueAgainstKey(CustomFieldsService.STORY_POINTS_CF_KEY);
            string teamBoardCfValue = await customFieldsService.GetCustomFieldValueAgainstKey(CustomFieldsService.TEAM_BOARD_CF_KEY);

            List<Issue> issues = new List<Issue>();
            // Select and transform issues
            foreach(var item in issuesObject)
            {
                Issue issue = new Issue();
                issue.Id = castValueToGivenType<string>(item["key"]);
                issue.IssueType = convertToIssueType(item["fields"]["issuetype"]);
                issue.IssueEstimatedAndSpentTime = convertTimeToEstimatedAndSpentTime(
                        item["fields"]["aggregatetimespent"],
                        item["fields"]["aggregatetimeoriginalestimate"]);
                issue.Summary = castValueToGivenType<string>(item["fields"]["summary"]);
                issue.CreatedDate = getFormattedDate(item["fields"]["created"]);
                issue.ResolvedDate = getFormattedDate(item["fields"]["resolutiondate"]);
                issue.Priority = castValueToGivenType<string>(item["fields"]["priority"]["name"]);
                issue.StoryPoints = castValueToGivenType<int>(item["fields"][storyPointsCfValue]);
                issue.Status = castValueToGivenType<string>(item["fields"]["status"]["name"]);
                issue.Parent = convertToParent(item["fields"]["parent"], sourceUrl);
                issue.TeamBoard = getTeamBoard(item["fields"][teamBoardCfValue]);
                issue.IssueUrl = prepareIssueUrl(sourceUrl, castValueToGivenType<string>(item["key"]));
                issue.ProductivityRatio = calculateProductivityRatio(item["fields"][storyPointsCfValue], item["fields"]["aggregatetimespent"]);
                issue.FixVersions = getReleaseList(item["fields"]["fixVersions"], issue);
                issues.Add(issue);
            }
            /*var issues = issuesObject.Select(item => {
                var issue = new Issue
                {
                    Id = castValueToGivenType<string>(item["key"]),
                    IssueType = convertToIssueType(item["fields"]["issuetype"]),
                    IssueEstimatedAndSpentTime = convertTimeToEstimatedAndSpentTime(
                        item["fields"]["aggregatetimespent"],
                        item["fields"]["aggregatetimeoriginalestimate"]),
                    Summary = castValueToGivenType<string>(item["fields"]["summary"]),
                    CreatedDate = getFormattedDate(item["fields"]["created"]),
                    ResolvedDate = getFormattedDate(item["fields"]["resolutiondate"]),
                    Priority = castValueToGivenType<string>(item["fields"]["priority"]["name"]),
                    StoryPoints = castValueToGivenType<int>(item["fields"][storyPointsCfValue]),
                    Status = castValueToGivenType<string>(item["fields"]["status"]["name"]),
                    Parent = convertToParent(item["fields"]["parent"], sourceUrl),
                    FixVersions = getReleaseList(item["fields"]["fixVersions"], issue),
                    TeamBoard = getTeamBoard(item["fields"][teamBoardCfValue]),
                    IssueUrl = prepareIssueUrl(sourceUrl, castValueToGivenType<string>(item["key"])),
                    ProductivityRatio = calculateProductivityRatio(item["fields"][storyPointsCfValue], item["fields"]["aggregatetimespent"])
                };
                return issue;
            });*/


            return new List<Issue>(issues);
        }
        

        private List<IssueRelease> getReleaseList(JToken jsonObject, Issue issue)
        {
            if (jsonObject.IsNullOrEmpty())
            {
                return null;
            }

            var list = new List<IssueRelease>();
            foreach (var item in jsonObject)
            {
                list.Add(new IssueRelease
                {
                    Issue = issue,
                    Release = new Release
                    {
                        Id = castValueToGivenType<string>(item["id"]),
                        Released = castValueToGivenType<bool>(item["released"]),
                        Name = castValueToGivenType<string>(item["name"]),
                        ReleaseDate = getFormattedDate(castValueToGivenType<string>(item["releaseDate"]), "yyyy-mm-dd")
                    }
                    /*Id = castValueToGivenType<string>(item["id"]),
                    Released = castValueToGivenType<bool>(item["released"]),
                    Name = castValueToGivenType<string>(item["name"]),
                    ReleaseDate = getFormattedDate(castValueToGivenType<string>(item["releaseDate"]), "yyyy-mm-dd")*/
                }) ;
            }

            return list;
        }
        
        private TeamBoard getTeamBoard(JToken jsonObject)
        {
            if (jsonObject.IsNullOrEmpty())
                return null;

            TeamBoard teamBoard = new TeamBoard
            {
                Name = castValueToGivenType<string>(jsonObject["value"]),
                Id = castValueToGivenType<string>(jsonObject["id"])
            };
            return teamBoard;
        }
        

        private Uri prepareIssueUrl(string sourceUrl, string issueId)
        {
            if (String.IsNullOrEmpty(issueId) || String.IsNullOrWhiteSpace(issueId))
            {
                return null;
            }

            return new Uri(new Uri(sourceUrl), $"browse/{issueId}");
        }

        
        private string getFormattedDate(JToken date, string timeFormat = "dd/MM/yyyy HH:mm:ss")
        {
            string formattedDate = date.ToString();
            if (String.IsNullOrEmpty(formattedDate) || String.IsNullOrWhiteSpace(formattedDate))
            {
                return formattedDate;
            }

            DateTime parsedDate = DateTime.ParseExact(formattedDate, timeFormat, 
                System.Globalization.CultureInfo.InvariantCulture);
            return parsedDate.ToString("dd/MM/yyyy");
        }

        public T castValueToGivenType<T>(JToken jToken)
        {
            try
            {
                if (jToken == null || (jToken.Type == JTokenType.String && string.IsNullOrWhiteSpace(jToken.ToString())))
                {
                    return default(T);
                }

                return jToken.ToObject<T>();
            }
            catch(Exception ex)
            {
                return default(T);
            }
        }
        
        private Parent? convertToParent(JToken jToken, string sourceUrl)
        {
            if(jToken.IsNullOrEmpty())
            {
                return null;
            }

            Parent parent = new Parent
            {
                Id = castValueToGivenType<string>(jToken["key"]),
                ParentUrl = prepareIssueUrl(sourceUrl, castValueToGivenType<string>(jToken["key"])),
                Summary = castValueToGivenType<string>(jToken["fields"]["summary"]),
                Status = castValueToGivenType<string>(jToken["fields"]["status"]["name"]),
                Priority = castValueToGivenType<string>(jToken["fields"]["priority"]["name"])
            };
            return parent;
        }

        private IssueType convertToIssueType(JToken jToken)
        {

            IssueType issueType = new IssueType
            {
                Name = castValueToGivenType<string>(jToken["name"]),
                SubTask = castValueToGivenType<bool>(jToken["subtask"]),
                Id = castValueToGivenType<string>(jToken["id"])
            };

            return issueType;
        }
        
        private EstimatedAndSpentTime convertTimeToEstimatedAndSpentTime(JToken jTokenEstimatedTime, JToken jTokenSpentTime)
        {
            int estimatedTime = castValueToGivenType<int>(jTokenEstimatedTime);
           int timeSpent = castValueToGivenType<int>(jTokenSpentTime);


            EstimatedAndSpentTime estimatedAndSpentTime = new EstimatedAndSpentTime
            {
                AggregateTimeEstimate = estimatedTime,
                AggregatedTimeEstimateInDays = calculateTimeInDays(estimatedTime),
                AggregatedTimeSpent = timeSpent,
                AggregatedTimeSpentInDays = calculateTimeInDays(timeSpent)
            };
            return estimatedAndSpentTime;
        }

        private int calculateTimeInDays(int timeInSeconds)
        {
            return timeInSeconds / (3600 * 8);
        }

        public async Task<List<string>> getFieldsValues()
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
                "fixVersions"
            };

            var storyPointsKey = await customFieldsService.GetCustomFieldValueAgainstKey(CustomFieldsService.STORY_POINTS_CF_KEY);
            var teamBoardsKey = await customFieldsService.GetCustomFieldValueAgainstKey(CustomFieldsService.TEAM_BOARD_CF_KEY);
            fields.Add(storyPointsKey);
            fields.Add(teamBoardsKey);

            return fields;
        }

        private double calculateProductivityRatio(JToken jTokenStoryPoints, JToken jTokenTimeSpent)
        {
            int storyPoints = castValueToGivenType<int>(jTokenStoryPoints);
            double timeSpent = castValueToGivenType<double>(jTokenTimeSpent);
            if(timeSpent == 0 || storyPoints == 0)
            {
                return 0;
            }
            timeSpent = timeSpent / 3600;
            return (double) timeSpent/storyPoints;  
        }

    }
}
