using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using WebApplication7.Models;

namespace WebApplication7.Services
{
    public class IssueMapperService
    {

        public async Task<List<Issue>> MapToIssueObject(JToken issuesObject, string sourceUrl, string storyPointsCfValue, string teamBoardCfValue)
        {
            List<Issue> issues = new List<Issue>();
            // Select and transform issues
            foreach(var item in issuesObject)
            {
                Issue issue = new Issue();
                issue.Id = castValueToGivenType<string>(item["key"]);
                issue.IssueType = convertToIssueType(item["fields"]["issuetype"]);
                issue.IssueEstimatedAndSpentTime = convertTimeToEstimatedAndSpentTime(
                        item["fields"]["aggregatetimespent"],
                        item["fields"]["aggregatetimeoriginalestimate"], issue.Id);
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
            string[] parts = formattedDate.Split(' ');

            // Check if there's at least one element (date part) after splitting
            if (parts.Length >= 1)
            {
                return parts[0]; // First element is the date portion
            }
            else
            {
                return formattedDate; // Return original string if no split
            }
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
        
        private EstimatedAndSpentTime convertTimeToEstimatedAndSpentTime(JToken jTokenSpentTime, JToken jTokenEstimatedTime, string id)
        {
            int estimatedTime = castValueToGivenType<int>(jTokenEstimatedTime);
            int timeSpent = castValueToGivenType<int>(jTokenSpentTime);

            EstimatedAndSpentTime estimatedAndSpentTime = new EstimatedAndSpentTime
            {
                AggregateTimeEstimate = estimatedTime,
                AggregatedTimeEstimateInDays = calculateTimeInDays(estimatedTime),
                AggregatedTimeSpent = timeSpent,
                AggregatedTimeSpentInDays = calculateTimeInDays(timeSpent),
                Id = id
            };
            return estimatedAndSpentTime;
        }

        private double calculateTimeInDays(int timeInSeconds)
        {
            return (double)timeInSeconds / (3600 * 8);
        }

        public async Task<List<string>> getFieldsValues(string storyPointsCfValue, string teamBoardsCfValue)
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

            fields.Add(storyPointsCfValue);
            fields.Add(teamBoardsCfValue);

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
            return (double) storyPoints/timeSpent;  
        }

        public List<CustomField> ConvertResponseToCustomFields(JArray jsonArray)
        {
            List<CustomField> customFields = new List<CustomField>();
            foreach(var jsonElement in jsonArray)
            {
                string cfValue = String.Empty;

                if(jsonElement["schema"]?["customId"]!=null)
                {
                    cfValue = castValueToGivenType<string>(jsonElement["schema"]["customId"]);
                }
                    
                customFields.Add(new CustomField
                {
                    CustomFieldKey = castValueToGivenType<string>(jsonElement["name"]),
                    CustomFieldValue = String.IsNullOrEmpty(cfValue) ? String.Empty : cfValue
                });
            }
            customFields = customFields.Where(customField => !String.IsNullOrWhiteSpace(customField.CustomFieldValue)).ToList();
            return customFields;
        }

    }
}
