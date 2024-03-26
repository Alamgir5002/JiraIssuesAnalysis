using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApplication7.Models;

namespace WebApplication7.Services
{
    public class IssueMapperService
    {
        public List<Issue> MapToIssueObject(string responseBody, string sourceUrl)
        {
            JObject jsonObject = JObject.Parse(responseBody);
            var issues = jsonObject["issues"]
                .Select(item => new Issue
                {
                    Id = castValueToGivenType<string>(item["key"]),
                    IssueType = castValueToGivenType<string>(item["fields"]["issuetype"]["name"]),
                    Description = castValueToGivenType<string>(item["fields"]["description"]),
                    AggregatedTimeSpent = castValueToGivenType<double>(item["fields"]?["aggregatetimespent"]),
                    AggregateTimeEstimate = castValueToGivenType<double>(item["fields"]?["aggregatetimeestimate"]),
                    Summary = castValueToGivenType<string>(item["fields"]["summary"]),
                    CreatedDate = getFormattedDate(item["fields"]["created"]),
                    ResolvedDate = getFormattedDate(item["fields"]["resolutiondate"]),
                    Priority = castValueToGivenType<string>(item["fields"]["priority"]["name"]),
                    StoryPoints = castValueToGivenType<int>(item["fields"]["customfield_10024"]),
                    Status = castValueToGivenType<string>(item["fields"]["status"]["name"]),
                    Parent = convertToParent(item["fields"]["parent"], sourceUrl),
                    FixVersions = getReleaseList(item["fields"]["fixVersions"]),
                    TeamBoard = getTeamBoard(item["fields"]?["customfield_10021"]),
                    IssueUrl = prepareIssueUrl(sourceUrl, castValueToGivenType<string>(item["key"]))
                });

            return issues.ToList();
        }

        private List<Release> getReleaseList(JToken jsonObject)
        {
            if (jsonObject.IsNullOrEmpty())
            {
                return null;
            }
            var list = new List<Release>();
            foreach (var item in jsonObject)
            {
                list.Add(new Release
                {
                    Id = castValueToGivenType<string>(item["id"]),
                    Released = castValueToGivenType<bool>(item["released"]),
                    Name = castValueToGivenType<string>(item["name"])
                });
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

        private string getFormattedDate(JToken date)
        {
            string formattedDate = castValueToGivenType<string>(date);
            if (String.IsNullOrEmpty(formattedDate) || String.IsNullOrWhiteSpace(formattedDate))
            {
                return formattedDate;
            }

            DateTime parsedDate = DateTime.ParseExact(formattedDate, "dd/MM/yyyy HH:mm:ss", 
                System.Globalization.CultureInfo.InvariantCulture);
            return parsedDate.ToString("dd/MM/yyyy");
        }

        private T castValueToGivenType<T>(JToken jToken)
        {
            if(jToken == null || jToken.Type == JTokenType.String && string.IsNullOrWhiteSpace(jToken.ToString()) || (jToken.IsNullOrEmpty() && jToken.Type != JTokenType.String))
            {
                return default(T);
            }

            return jToken.ToObject<T>();
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
                Priority = castValueToGivenType<string>(jToken["fields"]["priority"]["name"]),
                IssueType = castValueToGivenType<string>(jToken["fields"]["issuetype"]["name"])
            };
            return parent;
        }
    }
}
